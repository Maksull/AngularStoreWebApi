using Amazon.S3;
using Amazon.S3.Model;

namespace WebApi.Services.S3Service
{
    public sealed class S3Service : IS3Service
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;

        public S3Service(IConfiguration configuration, IAmazonS3 s3client)
        {
            _configuration = configuration;
            _s3Client = s3client;
        }

        public async Task<bool> AddImageToBucket(IFormFile file, string path)
        {
            if (await _s3Client.DoesS3BucketExistAsync(_configuration.GetSection("AWS:BucketName").Value))
            {
                PutObjectRequest request = new()
                {
                    BucketName = _configuration.GetSection("AWS:BucketName").Value,
                    Key = path,
                    InputStream = file.OpenReadStream(),
                };
                request.Metadata.Add("Content-Type", file.ContentType);
                await _s3Client.PutObjectAsync(request);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteImageFromBucket(string path)
        {
            if (await _s3Client.DoesS3BucketExistAsync(_configuration.GetSection("AWS:BucketName").Value))
            {
                await _s3Client.DeleteObjectAsync(_configuration.GetSection("AWS:BucketName").Value, path);
                return true;
            }
            return false;
        }
    }
}
