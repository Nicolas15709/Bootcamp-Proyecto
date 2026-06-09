using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldCupStickers.Models;

public enum EstadoCromo
{
    Nuevo,
    Repetido,
    Intercambiable,
    Favorito
}

public class UsuarioCromo
{
    public int UsuarioId { get; set; }

    public int CromoId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de adquisición")]
    public DateTime FechaAdquisicion { get; set; }

    [Display(Name = "Estado")]
    public EstadoCromo Estado { get; set; }

    // Relaciones
    [ForeignKey(nameof(UsuarioId))]
    public Usuario? Usuario { get; set; }

    [ForeignKey(nameof(CromoId))]
    public Cromo? Cromo { get; set; }
}
