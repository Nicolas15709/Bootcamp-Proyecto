using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldCupStickers.Models;

public class Cromo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El número de cromo es obligatorio.")]
    [Range(1, 9999, ErrorMessage = "El número de cromo debe ser mayor a 0.")]
    [Display(Name = "Número de cromo")]
    public int NumeroCromo { get; set; }

    [Required(ErrorMessage = "La edición es obligatoria.")]
    [StringLength(80)]
    public string Edicion { get; set; } = string.Empty;

    [Range(0, 1000000, ErrorMessage = "El valor de mercado debe ser mayor o igual a 0.")]
    [Display(Name = "Valor de mercado")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorMercado { get; set; }

    [StringLength(300)]
    [Display(Name = "Foto del cromo")]
    public string? FotoUrl { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un jugador.")]
    [Display(Name = "Jugador")]
    public int JugadorId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un equipo.")]
    [Display(Name = "Equipo")]
    public int EquipoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un álbum.")]
    [Display(Name = "Álbum")]
    public int AlbumId { get; set; }

    // Relaciones
    [ForeignKey(nameof(JugadorId))]
    public Jugador? Jugador { get; set; }

    [ForeignKey(nameof(EquipoId))]
    public Equipo? Equipo { get; set; }

    [ForeignKey(nameof(AlbumId))]
    public Album? Album { get; set; }

    public ICollection<UsuarioCromo> UsuarioCromos { get; set; } = new List<UsuarioCromo>();
}
