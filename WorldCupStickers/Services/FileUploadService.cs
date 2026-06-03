namespace WorldCupStickers.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _environment;

    private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long TamanioMaximoBytes = 2 * 1024 * 1024; // 2 MB

    public FileUploadService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder)
    {
        if (file is null || file.Length == 0)
            throw new InvalidOperationException("El archivo está vacío.");

        if (file.Length > TamanioMaximoBytes)
            throw new InvalidOperationException("La imagen supera el tamaño máximo permitido de 2 MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!ExtensionesPermitidas.Contains(extension))
            throw new InvalidOperationException("Formato no permitido. Solo se aceptan JPG, PNG o WEBP.");

        // Nombre único con Guid para evitar colisiones y nombres maliciosos
        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var carpetaDestino = Path.Combine(_environment.WebRootPath, "images", folder);
        Directory.CreateDirectory(carpetaDestino);

        var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);
        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Ruta relativa servible desde el navegador
        return $"/images/{folder}/{nombreArchivo}";
    }

    public void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return;

        // Solo borrar archivos locales (no URLs externas)
        if (imagePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return;

        var rutaCompleta = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(rutaCompleta))
            File.Delete(rutaCompleta);
    }
}
