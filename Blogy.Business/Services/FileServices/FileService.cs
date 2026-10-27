using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Blogy.Business.Services.FileServices;

public class FileService(IWebHostEnvironment environment) : IFileService
{
    public async Task<string> UploadFileAsync(IFormFile file, string folderName)
    {
        var extension = Path.GetExtension(file.FileName);
        var newFileName = $"{Guid.NewGuid()}{extension}";
        
        var folderPath = Path.Combine(environment.WebRootPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        
        var fullPath = Path.Combine(folderPath, newFileName);
        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{folderName}/{newFileName}";
    }

    public void DeleteFile(string filePath)
    {
        if(string.IsNullOrEmpty(filePath)) return;
        
        var fullPath = Path.Combine(environment.WebRootPath, filePath.TrimStart('/'));
        
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}