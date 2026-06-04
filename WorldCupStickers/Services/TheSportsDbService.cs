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

    // URL base de la API pública (clave 3 = acceso gratuito)
    private const string BaseUrl = "https://www.thesportsdb.com/api/v1/json/3";

    public TheSportsDbService(HttpClient http, ILogger<TheSportsDbService> logger)
    {
        _http = http;
        _http.BaseAddress = new Uri(BaseUrl + "/");
        _http.Timeout = TimeSpan.FromSeconds(10);
        _http.DefaultRequestHeaders.Add("User-Agent", "WorldCupStickerManager/1.0");
    }

    public async Task<TheSportsDbJugador?> BuscarJugadorAsync(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return null;

        try
        {
            // Endpoint: /searchplayers.php?p={nombre}
            var url = $"searchplayers.php?p={Uri.EscapeDataString(nombre.Trim())}";
            using var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // La API devuelve: { "player": [ {...}, ... ] }
            // Si no hay resultados devuelve: { "player": null }
            if (!doc.RootElement.TryGetProperty("player", out var arr)
                || arr.ValueKind != JsonValueKind.Array
                || arr.GetArrayLength() == 0)
                return null;

            // Tomar el primer resultado
            var p = arr[0];

            return new TheSportsDbJugador
            {
                Nombre      = GetStr(p, "strPlayer") ?? nombre,
                FotoUrl     = GetStr(p, "strThumb"),
                Equipo      = GetStr(p, "strTeam"),
                Posicion    = GetStr(p, "strPosition"),
                Nacionalidad = GetStr(p, "strNationality")
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "TheSportsDB: error buscando jugador '{Nombre}'", nombre);
            return null;
        }
    }

    public async Task<string?> BuscarFotoAsync(string nombre)
    {
        var jugador = await BuscarJugadorAsync(nombre);
        return jugador?.FotoUrl;
    }

    // Helper para leer strings de JsonElement sin lanzar excepción
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
