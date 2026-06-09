using System.ComponentModel.DataAnnotations;

namespace WorldCupStickers.Models;

public class Album
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre del álbum")]
    public string Nombre { get; set; } = string.Empty;

    [Range(1930, 2100, ErrorMessage = "El año debe estar entre 1930 y 2100.")]
    public int Anio { get; set; }

    [Range(1, 10000, ErrorMessage = "La cantidad de cromos debe ser mayor a 0.")]
    [Display(Name = "Cantidad de cromos")]
    public int CantidadCromos { get; set; }

    [Display(Name = "Edición especial")]
    public bool EdicionEspecial { get; set; }

    // Relaciones
    public ICollection<Cromo> Cromos { get; set; } = new List<Cromo>();
}
