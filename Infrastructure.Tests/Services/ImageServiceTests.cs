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

        #region GetImage

        [Fact]
        public async Task GetImage_WhenImageExists_ReturnGetObjectResponse()
        {
            // Arrange
            GetObjectResponse getObjectResponse = new();

            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _s3Client.Setup(s => s.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(getObjectResponse);

            // Act
            var result = (await _imageService.GetImage(""))!;

            // Assert
            result.Should().BeOfType<GetObjectResponse>();
            result.Should().BeEquivalentTo(getObjectResponse);
        }

        [Fact]
        public async Task GetImage_WhenImageNotExists_ReturnGetObjectResponse()
        {
            // Arrange
            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _s3Client.Setup(s => s.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync((GetObjectResponse)null!);

            // Act
            var result = (await _imageService.GetImage(""))!;

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetImage_WhenBucketDoesNotExists_ReturnNull()
        {
            // Arrange
            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = (await _imageService.GetImage(""))!;

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetImage_WhenException_ReturnNull()
        {
            // Arrange
            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test Exception"));

            _s3Client.Setup(s => s.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync((GetObjectResponse)null!);

            // Act
            var result = (await _imageService.GetImage(""))!;

            // Assert
            result.Should().BeNull();
        }

        #endregion


        #region UploadImage

        [Fact]
        public async Task UploadImage_WhenBucketExists_ReturnImage()
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
            var result = (await _imageService.UploadImage(file, ""))!;

            // Assert
            result.Should().BeOfType<FormFile>();
            result.Should().BeEquivalentTo(file);
        }

        [Fact]
        public async Task UploadImage_WhenBucketNotExists_ReturnNull()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = (await _imageService.UploadImage(file, ""))!;

            // Assert
            result.Should().BeNull();
        }

        #endregion


        #region DeleteImage

        [Fact]
        public async Task DeleteImage_WhenBucketExists_ReturnTrue()
        {
            // Arrange
            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _imageService.DeleteImage("");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteImage_WhenBucketNotExists_ReturnFalse()
        {
            // Arrange
            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _imageService.DeleteImage("");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteImage_WhenException_ReturnFalse()
        {
            // Arrange
            _s3Client.Setup(s => s.DoesS3BucketExistAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _s3Client.Setup(s => s.DeleteObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _imageService.DeleteImage("");

            // Assert
            result.Should().BeFalse();
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
