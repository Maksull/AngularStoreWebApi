using Microsoft.AspNetCore.Http;
using Amazon.S3.Model;

namespace Infrastructure.Services.Interfaces
{
    public interface IImageService
    {
        Task<GetObjectResponse?> GetImage(string key);
        Task<IFormFile?> UploadImage(IFormFile file, string path);
        Task<bool> DeleteImage(string path);
    }
}
