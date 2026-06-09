namespace WorldCupStickers.Services;

public interface IFileUploadService
{
    /// <summary>
    /// Guarda la imagen en wwwroot/images/{folder} con un nombre Guid y devuelve la ruta relativa.
    /// </summary>
    Task<string> UploadImageAsync(IFormFile file, string folder);

    /// <summary>
    /// Elimina una imagen previamente subida a partir de su ruta relativa.
    /// </summary>
    void DeleteImage(string? imagePath);
}
