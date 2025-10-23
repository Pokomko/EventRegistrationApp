using Application.Interfaces;

namespace Web.Servicies;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }
    public async Task<string> SaveEventImageAsync(IFormFile file)
    {
        if (file != null) {
            var imagesFolder = Path.Combine(_env.WebRootPath, "images/events");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(imagesFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/events/{fileName}";
        }

        throw new ArgumentNullException(nameof(file), "File cannot be null");
    }
}
