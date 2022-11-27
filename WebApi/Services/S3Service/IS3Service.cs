namespace WebApi.Services.S3Service
{
    public interface IS3Service
    {
        Task<bool> AddImageToBucket(IFormFile file, string path);
        Task<bool> DeleteImageFromBucket(string path);
    }
}
