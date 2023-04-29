using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text;

namespace Infrastructure.Tests.Services
{
    public sealed class ImageServiceTests
    {
        private readonly Mock<IAmazonS3> _s3Client;
        private readonly IConfiguration _configuration;
        private readonly ImageService _imageService;


        public ImageServiceTests()
        {
            _s3Client = new();
            _configuration = GetConfiguration();
            _imageService = new(_s3Client.Object, _configuration);
        }

        #region GetFile

        [Fact]
        public async Task GetFile_WhenFileExists_ReturnGetObjectResponse()
        {
            // Arrange
            GetObjectResponse getObjectResponse = new();

            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _s3Client.Setup(s => s.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(getObjectResponse);

            // Act
            var result = (await _imageService.GetFile(""))!;

            // Assert
            result.Should().BeOfType<GetObjectResponse>();
            result.Should().BeEquivalentTo(getObjectResponse);
        }

        [Fact]
        public async Task GetFile_WhenFileNotExists_ReturnGetObjectResponse()
        {
            // Arrange
            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _s3Client.Setup(s => s.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync((GetObjectResponse)null!);

            // Act
            var result = (await _imageService.GetFile(""))!;

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetFile_WhenException_ReturnNull()
        {
            // Arrange
            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test Exception"));

            _s3Client.Setup(s => s.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync((GetObjectResponse)null!);

            // Act
            var result = (await _imageService.GetFile(""))!;

            // Assert
            result.Should().BeNull();
        }

        #endregion


        #region UploadFile

        [Fact]
        public async Task UploadFile_WhenBucketExists_ReturnFile()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = (await _imageService.UploadFile(file))!;

            // Assert
            result.Should().BeOfType<FormFile>();
            result.Should().BeEquivalentTo(file);
        }

        [Fact]
        public async Task GetFile_WhenBucketNotExists_Null()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = (await _imageService.UploadFile(file))!;

            // Assert
            result.Should().BeNull();
        }

        #endregion


        private static IConfiguration GetConfiguration()
        {
            Dictionary<string, string?> inMemorySettings = new() {
                {"JwtSettings:SecurityKey", "MyTestsAuthServiceSecurityKey"},
                {"JwtSettings:ExpiresInMinutes", "30"},
            };

            return new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }
    }
}
