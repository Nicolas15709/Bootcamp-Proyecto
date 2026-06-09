using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldCupStickers.Models;

public class Equipo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre del equipo")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El director técnico es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Director técnico")]
    public string DirectorTecnico { get; set; } = string.Empty;

    [Range(1850, 2100, ErrorMessage = "El año de fundación debe estar entre 1850 y 2100.")]
    [Display(Name = "Año de fundación")]
    public int AnioFundacion { get; set; }

    [StringLength(300)]
    [Display(Name = "Logo del equipo")]
    public string? LogoUrl { get; set; }

    [Required(ErrorMessage = "El grupo mundialista es obligatorio.")]
    [StringLength(2)]
    [Display(Name = "Grupo mundialista")]
    public string GrupoMundialista { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un país.")]
    [Display(Name = "País")]
    public int PaisId { get; set; }

    // Relaciones
    [ForeignKey(nameof(PaisId))]
    public Pais? Pais { get; set; }

    public ICollection<Jugador> Jugadores { get; set; } = new List<Jugador>();
    public ICollection<Cromo> Cromos { get; set; } = new List<Cromo>();
}
