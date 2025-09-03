using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Contracts.Base;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Service.Base;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
    private const int MaxFileSize = 2 * 1024 * 1024; // 2MB
    private const string DefaultImagePath = "/DefaultImage/default-photo.jpg";

    public FileService(IWebHostEnvironment env) => _env = env;

    public async Task<string> SaveFileAsync(IFormFile? file, string dirName)
    {
        if (file == null || file.Length == 0)
            return DefaultImagePath;

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedExtensions.Contains(extension))
            throw new Exception("Invalid file type. Allowed: .jpg, .jpeg, .png, .gif, .pdf");

        if (file.Length > MaxFileSize)
            throw new Exception("File size exceeds 2MB.");

        var originalFolderPath =
            Path.Combine(_env.WebRootPath, "files", "images", dirName, "original");
        Directory.CreateDirectory(originalFolderPath);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var originalFilePath = Path.Combine(originalFolderPath, fileName);

        using (var stream = new FileStream(originalFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        ResizeImage(originalFilePath, dirName, "small", 32, 32);
        ResizeImage(originalFilePath, dirName, "medium", 300, 300);
        ResizeImage(originalFilePath, dirName, "large", 600, 600);

        return $"/files/images/{dirName}/original/{fileName}";
    }

    private void ResizeImage(string originalFilePath, string dirName, string sizeFolder, int width, int height)
    {
        var sizePath = Path.Combine(_env.WebRootPath, "files", "images", dirName, sizeFolder);
        Directory.CreateDirectory(sizePath);

        var resizedFilePath = Path.Combine(sizePath, Path.GetFileName(originalFilePath));

        using var image = Image.Load(originalFilePath);

        //image.Mutate(x => x.Resize(width, height));

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(width, height)
        }));

        int quality = sizeFolder.ToLower() switch
        {
            "small" => 50,
            "medium" => 70,
            "large" => 85,
            _ => 75
        };

        IImageEncoder encoder;
        var extension = Path.GetExtension(originalFilePath).ToLower();

        if (extension == ".jpg" || extension == ".jpeg")
        {
            encoder = new JpegEncoder { Quality = quality };
        }
        else if (extension == ".png")
        {
            encoder = new PngEncoder
            {
                CompressionLevel = PngCompressionLevel.BestCompression
            };
        }
        else
        {
            encoder = new JpegEncoder { Quality = quality };
        }

        image.Save(resizedFilePath, encoder);
        //image.Save(resizedFilePath, new JpegEncoder());
    }

    public async Task DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || filePath == DefaultImagePath) return;

        var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            await Task.CompletedTask;
        }
    }
}
