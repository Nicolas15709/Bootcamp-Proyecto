using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldCupStickers.Models;

public class Jugador
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre del jugador")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La posición es obligatoria.")]
    [StringLength(50)]
    public string Posicion { get; set; } = string.Empty;

    [Range(1, 99, ErrorMessage = "El número de camiseta debe estar entre 1 y 99.")]
    [Display(Name = "Número de camiseta")]
    public int NumeroCamiseta { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de nacimiento")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un equipo.")]
    [Display(Name = "Equipo")]
    public int EquipoId { get; set; }

    // Relaciones
    [ForeignKey(nameof(EquipoId))]
    public Equipo? Equipo { get; set; }

    public ICollection<Cromo> Cromos { get; set; } = new List<Cromo>();
}
