﻿using Amazon.S3.Model;
using Core.Mediator.Queries.Images;
using Infrastructure.Mediator.Handlers.Images;
using Infrastructure.Services.Interfaces;
using Moq;
using System.Text;

namespace Infrastructure.Tests.Mediator.Handlers.Images
{
    public sealed class GetImageHandlerTests
    {
        private readonly Mock<IImageService> _service;
        private readonly GetImageHandler _handler;

        public GetImageHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnGetObjectResponse()
        {
            //Arrange
            GetObjectResponse getObjectResponse = new()
            {
                ResponseStream = new MemoryStream(Encoding.UTF8.GetBytes("test")),
            };
            _service.Setup(s => s.GetImage(It.IsAny<string>()))
                .ReturnsAsync(getObjectResponse);

            //Act
            var result = _handler.Handle(new GetImageQuery(""), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<GetObjectResponse>();
            result.Should().BeEquivalentTo(getObjectResponse);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            GetObjectResponse getObjectResponse = new()
            {
                ResponseStream = new MemoryStream(Encoding.UTF8.GetBytes("test")),
            };
            _service.Setup(s => s.GetImage(It.IsAny<string>()))
                .ReturnsAsync((GetObjectResponse)null!);

            //Act
            var result = _handler.Handle(new GetImageQuery(""), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}