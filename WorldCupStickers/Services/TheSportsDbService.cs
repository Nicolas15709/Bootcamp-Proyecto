using System.Globalization;
using System.Text;
using System.Text.Json;

namespace WorldCupStickers.Services;

/// <summary>
/// Implementación que consume la API gratuita de TheSportsDB (clave pública "3").
/// Docs: https://www.thesportsdb.com/api.php
/// </summary>
public class TheSportsDbService : ITheSportsDbService
{
    private readonly HttpClient _http;
    private readonly ILogger<TheSportsDbService> _logger;

    private const string BaseUrl = "https://www.thesportsdb.com/api/v1/json/3";

    public TheSportsDbService(HttpClient http, ILogger<TheSportsDbService> logger)
    {
        _http = http;
        _logger = logger;
        _http.BaseAddress = new Uri(BaseUrl + "/");
        _http.Timeout = TimeSpan.FromSeconds(12);
        _http.DefaultRequestHeaders.Add("User-Agent", "WorldCupStickerManager/1.0");
    }

    public async Task<TheSportsDbJugador?> BuscarJugadorAsync(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return null;

        // Genera variantes del nombre para maximizar la tasa de aciertos
        var variantes = GenerarVariantes(nombre.Trim());

        foreach (var variante in variantes)
        {
            var resultado = await BuscarExactoAsync(variante);
            if (resultado != null) return resultado;
        }
        return null;
    }

    public async Task<string?> BuscarFotoAsync(string nombre)
    {
        var jugador = await BuscarJugadorAsync(nombre);
        return jugador?.FotoUrl;
    }

    // ──────────────────────────────────────────────────────────────
    // Privados
    // ──────────────────────────────────────────────────────────────

    private async Task<TheSportsDbJugador?> BuscarExactoAsync(string nombre)
    {
        try
        {
            var url = $"searchplayers.php?p={Uri.EscapeDataString(nombre)}";
            using var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("player", out var arr)
                || arr.ValueKind != JsonValueKind.Array
                || arr.GetArrayLength() == 0)
                return null;

            var p = arr[0];
            return new TheSportsDbJugador
            {
                Nombre       = GetStr(p, "strPlayer") ?? nombre,
                FotoUrl      = GetStr(p, "strThumb"),
                Equipo       = GetStr(p, "strTeam"),
                Posicion     = GetStr(p, "strPosition"),
                Nacionalidad = GetStr(p, "strNationality")
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "TheSportsDB: error buscando '{Nombre}'", nombre);
            return null;
        }
    }

    /// <summary>
    /// Genera hasta 4 variantes del nombre para buscar:
    /// 1. Nombre original
    /// 2. Sin diacríticos (tildes, diéresis, ñ…)
    /// 3. Solo el apellido (última palabra)
    /// 4. Solo el primer nombre
    /// </summary>
    private static IEnumerable<string> GenerarVariantes(string nombre)
    {
        var vistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        void Yield(string v)
        {
            if (!string.IsNullOrWhiteSpace(v) && vistos.Add(v))
                vistos.Add(v); 
        }

        Yield(nombre);
        Yield(QuitarDiacriticos(nombre));

        var partes = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length > 1)
        {
            Yield(partes[^1]);
            Yield(QuitarDiacriticos(partes[^1]));
            Yield(partes[0]);
            Yield(QuitarDiacriticos(partes[0]));
            if (partes.Length >= 2)
                Yield(partes[^1] + " " + partes[0]);
        }

        return vistos;
    }

    /// <summary>
    /// Convierte "Müller" → "Muller", "Félix" → "Felix", "Rüdiger" → "Rudiger", etc.
    /// </summary>
    private static string QuitarDiacriticos(string texto)
    {
        var normalizado = texto.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalizado.Length);
        foreach (var c in normalizado)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string? GetStr(JsonElement el, string prop)
    {
        if (el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.String)
        {
            var s = v.GetString();
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }
        return null;
    }
}
