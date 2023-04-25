using Amazon.S3.Model;
using Core.Mediator.Commands.Images;
using Core.Mediator.Queries.Images;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers
{
    public sealed class ImagesControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ImagesController _controller;

        public ImagesControllerTests()
        {
            _mediator = new();
            _controller = new(_mediator.Object);
        }


        #region UploadFile

        [Fact]
        public void UploadFile_WhenCalled_ReturnOk()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

            _mediator.Setup(m => m.Send(It.IsAny<UploadFileCommand>(), default))
                .ReturnsAsync(file);

            //Act
            var response = (_controller.UploadFile(file).Result as OkObjectResult)!;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            response.Value.Should().BeOfType<string>();
        }

        [Fact]
        public void UploadFile_WhenCalled_ReturnNotFound()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

            _mediator.Setup(m => m.Send(It.IsAny<UploadFileCommand>(), default))
                .ReturnsAsync((IFormFile)null!);

            //Act
            var response = _controller.UploadFile(file).Result;
            var result = (response as NotFoundResult)!;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void UploadFile_WhenException_ReturnProblem()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

            _mediator.Setup(m => m.Send(It.IsAny<UploadFileCommand>(), default))
                .Throws(new Exception("Test Exception"));

            //Act
            var response = (_controller.UploadFile(file).Result as ObjectResult)!;
            var result = response.Value as ProblemDetails;

            //Assert
            result.Should().BeOfType<ProblemDetails>();
            result.Should().Match<ProblemDetails>(r => r.Status == StatusCodes.Status500InternalServerError
                                                  && r.Detail == "Test Exception");
        }

        #endregion


        #region GetFile

        [Fact]
        public void GetFile_WhenCalled_ReturnOk()
        {
            //Arrange
            GetObjectResponse getObjectResponse = new()
            {
                ResponseStream = new MemoryStream(Encoding.UTF8.GetBytes("test")),
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetFileQuery>(), default))
                .ReturnsAsync(getObjectResponse);

            //Act
            var response = (_controller.GetFile("").Result as FileStreamResult)!;

            //Assert
            response.Should().BeOfType<FileStreamResult>();
            response.ContentType.Should().BeEquivalentTo("image/png");
            response.FileStream.Should().BeSameAs(getObjectResponse.ResponseStream);
        }

        [Fact]
        public void GetFile_WhenCalled_ReturnBadRequest()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetFileQuery>(), default))
                .ReturnsAsync((GetObjectResponse)null!);

            //Act
            var response = _controller.GetFile("").Result;
            var result = (response as NotFoundResult)!;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetFile_WhenException_ReturnProblem()
        {
            //Arrange
            GetObjectResponse getObjectResponse = new();

            _mediator.Setup(m => m.Send(It.IsAny<GetFileQuery>(), default))
                .Throws(new Exception("Test Exception"));

            //Act
            var response = (_controller.GetFile("").Result as ObjectResult)!;
            var result = response.Value as ProblemDetails;

            //Assert
            result.Should().BeOfType<ProblemDetails>();
            result.Should().Match<ProblemDetails>(r => r.Status == StatusCodes.Status500InternalServerError
                                                  && r.Detail == "Test Exception");
        }

        #endregion
    }
}
