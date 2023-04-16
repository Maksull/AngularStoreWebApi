using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Interfaces
{
    public interface IS3Service
    {
        Task<bool> AddImageToBucket(IFormFile file, string path);
        Task<bool> DeleteImageFromBucket(string path);
    }
}
