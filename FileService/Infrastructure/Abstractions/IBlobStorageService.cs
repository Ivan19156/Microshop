using Microsoft.AspNetCore.Http;

namespace FileService.Infrastructure.Abstractions;

public interface IBlobStorageService
{
    Task<string> UploadAsync(IFormFile file);
    Task DeleteAsync(string fileUrl); // опціонально
}

