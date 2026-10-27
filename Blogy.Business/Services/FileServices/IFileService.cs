using Microsoft.AspNetCore.Http;

namespace Blogy.Business.Services.FileServices;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folderName);
    void DeleteFile(string filePath);
}