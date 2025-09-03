using Microsoft.AspNetCore.Http;

namespace Service.Contracts.Base;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string dirName);

    Task DeleteFileAsync(string filePath);
}
