using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public sealed class ImageService : IImageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _s3BucketName;

        public ImageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _s3BucketName = configuration.GetSection("AWS:BucketName").Value!;
        }

        public async Task<GetObjectResponse?> GetFile(string key)
        {
            try
            {
                if (await _s3Client.DoesS3BucketExistAsync(_s3BucketName))
                {
                    GetObjectResponse s3Object = await _s3Client.GetObjectAsync(_s3BucketName, key);

                    return s3Object;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IFormFile?> UploadFile(IFormFile file)
        {
            if (await _s3Client.DoesS3BucketExistAsync(_s3BucketName))
            {
                PutObjectRequest request = new()
                {
                    BucketName = _s3BucketName,
                    Key = $"{file.FileName}",
                    InputStream = file.OpenReadStream(),
                };
                request.Metadata.Add("Content-Type", file.ContentType);
                await _s3Client.PutObjectAsync(request);

                return file;
            }

            return null;
        }
    }
}
