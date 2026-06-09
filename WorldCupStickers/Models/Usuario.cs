using System.ComponentModel.DataAnnotations;

namespace WorldCupStickers.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Relaciones
    public ICollection<UsuarioCromo> UsuarioCromos { get; set; } = new List<UsuarioCromo>();
}
