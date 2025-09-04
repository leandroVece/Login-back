using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Webp;

namespace Application.Helpers;

public static class Img
{

    public static async Task<byte[]> ConvertToWebP(Stream imageStream)
    {
        using Image image = await Image.LoadAsync(imageStream);
        using var memoryStream = new MemoryStream();

        var encoder = new WebpEncoder { Quality = 75 }; // Calidad ajustable
        await image.SaveAsync(memoryStream, encoder);

        return memoryStream.ToArray(); // Imagen convertida lista para enviar
    }
}

public static class ImageService
{
    public static async Task SaveToWebP(IFormFile imageStream, string webpFileName)
    {
        try
        {
            using var stream = imageStream.OpenReadStream();
            using Image image = await Image.LoadAsync(stream);

            // Definir carpeta de almacenamiento
            string uploadPath = Path.Combine("wwwroot/images/webp");

            // Crear la carpeta si no existe
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generar un nombre de archivo único basado en el original
            string fullPath = Path.Combine(uploadPath, webpFileName);

            // Configurar compresión y calidad del WebP
            var encoder = new WebpEncoder { Quality = 75 }; // Ajusta la calidad según necesites
            await image.SaveAsync(fullPath, encoder);

            // Retornar el nombre del archivo guardado
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al convertir la imagen: {ex.Message}");
        }
    }


    public static async Task<bool> DeleteWebP(string webpFileName)
    {
        try
        {
            string uploadPath = Path.Combine("wwwroot/images/webp");
            string fullPath = Path.Combine(uploadPath, webpFileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar la imagen: {ex.Message}");
            return false;
        }
    }

}