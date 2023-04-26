using Amazon.S3.Model;
using Core.Mediator.Commands.Images;
using Core.Mediator.Queries.Images;
using Infrastructure.Mediator.Handlers.Images;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Infrastructure.Tests.Mediator.Handlers.Images
{
    public sealed class UploadFileHandlerTests
    {
        private readonly Mock<IImageService> _service;
        private readonly UploadFileHandler _handler;

        public UploadFileHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnIFormFile()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            _service.Setup(s => s.UploadFile(It.IsAny<IFormFile>()))
                .ReturnsAsync(file);

            //Act
            var result = _handler.Handle(new UploadFileCommand(file), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<FormFile>();
            result.Should().BeEquivalentTo(file);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            _service.Setup(s => s.UploadFile(It.IsAny<IFormFile>()))
                .ReturnsAsync((IFormFile)null!);

            //Act
            var result = _handler.Handle(new UploadFileCommand(file), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
