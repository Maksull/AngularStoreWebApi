using Microsoft.AspNetCore.Http;
using Amazon.S3.Model;

namespace Infrastructure.Services.Interfaces
{
    public interface IImageService
    {
        Task<GetObjectResponse?> GetFile(string key);
        Task<IFormFile?> UploadFile(IFormFile file, string path);
        Task<bool> DeleteFile(string path);
    }
}
