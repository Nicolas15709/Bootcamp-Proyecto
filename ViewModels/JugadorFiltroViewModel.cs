using Microsoft.AspNetCore.Mvc.Rendering;
using WorldCupStickers.Models;

namespace WorldCupStickers.ViewModels;

public class JugadorFiltroViewModel
{
    public IEnumerable<Jugador> Jugadores { get; set; } = new List<Jugador>();

    // Filtros
    public string? Nombre { get; set; }
    public int? PaisId { get; set; }
    public int? EquipoId { get; set; }
    public string? Posicion { get; set; }

    // Listas para los selects
    public SelectList? Paises { get; set; }
    public SelectList? Equipos { get; set; }
    public SelectList? Posiciones { get; set; }
}
