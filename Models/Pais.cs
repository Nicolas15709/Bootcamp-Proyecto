using System.ComponentModel.DataAnnotations;

namespace WorldCupStickers.Models;

public class Pais
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Nombre del país")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El continente es obligatorio.")]
    [StringLength(50)]
    public string Continente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El código FIFA es obligatorio.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "El código FIFA debe tener exactamente 3 letras.")]
    [RegularExpression("^[A-Za-z]{3}$", ErrorMessage = "El código FIFA debe contener solo 3 letras.")]
    [Display(Name = "Código FIFA")]
    public string CodigoFifa { get; set; } = string.Empty;

    [Range(1, 300, ErrorMessage = "El ranking FIFA debe ser mayor a 0.")]
    [Display(Name = "Ranking FIFA")]
    public int RankingFifa { get; set; }

    // Relaciones
    public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}
