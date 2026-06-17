using WorldCupStickers.Models;

namespace WorldCupStickers.ViewModels;

public class DashboardViewModel
{
    public int TotalPaises { get; set; }
    public int TotalEquipos { get; set; }
    public int TotalJugadores { get; set; }
    public int TotalCromos { get; set; }
    public int TotalAlbumes { get; set; }

    // Colección de usuario logueado
    public int MisCromos { get; set; }
    public int MisRepetidos { get; set; }
    public int MisFaltan { get; set; }

    public IEnumerable<Cromo> UltimosCromos { get; set; } = new List<Cromo>();
    public IEnumerable<Equipo> EquiposDestacados { get; set; } = new List<Equipo>();
}
