using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public sealed class ImagesController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;

        private readonly string _s3BucketName;

        public ImagesController(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _configuration = configuration;
            _s3BucketName = _configuration.GetSection("AWS:BucketName").Value!;
        }

        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadFile(IFormFile file)
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
                return Ok($"File {file.FileName} uploaded to S3 successfully!");
            }
            return NotFound($"Bucket {_s3BucketName} does not exist");
        }

        [HttpGet("request")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFile(string key)
        {
            if (await _s3Client.DoesS3BucketExistAsync(_s3BucketName))
            {
                GetObjectResponse s3Object;
                try
                {
                    s3Object = await _s3Client.GetObjectAsync(_s3BucketName, key);
                }
                catch (Exception)
                {
                    return NotFound($"File {key} does not exist");
                }
                return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
            }
            return NotFound($"Bucket {_s3BucketName} does not exist");
        }

    }
}
