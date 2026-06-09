namespace WorldCupStickers.Services;

/// <summary>
/// Datos de un jugador devueltos por TheSportsDB.
/// </summary>
public class TheSportsDbJugador
{
    public string Nombre { get; set; } = "";
    public string? FotoUrl  { get; set; }   // strThumb
    public string? Equipo   { get; set; }
    public string? Posicion { get; set; }
    public string? Nacionalidad { get; set; }
}

/// <summary>
/// Servicio para consultar la API pública de TheSportsDB.
/// </summary>
public interface ITheSportsDbService
{
    /// <summary>Busca un jugador por nombre y devuelve su foto (strThumb) o null si no se encuentra.</summary>
    Task<string?> BuscarFotoAsync(string nombre);

    /// <summary>Busca un jugador por nombre y devuelve sus datos completos, o null si no existe.</summary>
    Task<TheSportsDbJugador?> BuscarJugadorAsync(string nombre);
}
