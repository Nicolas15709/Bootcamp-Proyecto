using Microsoft.AspNetCore.Mvc.Rendering;
using WorldCupStickers.Models;

namespace WorldCupStickers.ViewModels;

public class CromoFiltroViewModel
{
    public IEnumerable<Cromo> Cromos { get; set; } = new List<Cromo>();

    // Filtros
    public int? NumeroCromo { get; set; }
    public string? Jugador { get; set; }
    public int? PaisId { get; set; }
    public int? AlbumId { get; set; }
    public string? Edicion { get; set; }

    // Listas para los selects
    public SelectList? Paises { get; set; }
    public SelectList? Albumes { get; set; }
    public SelectList? Ediciones { get; set; }
}
