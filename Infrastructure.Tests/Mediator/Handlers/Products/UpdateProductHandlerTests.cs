using App.Metrics;
using App.Metrics.Counter;
using Core.Contracts.Controllers.Products;
using Core.Entities;
using Core.Mediator.Commands.Products;
using Infrastructure.Mediator.Handlers.Products;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Infrastructure.Tests.Mediator.Handlers.Products
{
    public sealed class UpdateProductHandlerTests
    {
        private readonly Mock<IProductService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnProduct()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            UpdateProductRequest updateProduct = new(1, "First", "Desc", 1, 1, 1, file);
            Product product = new()
            {
                Name = "First",
            };
            _service.Setup(s => s.UpdateProduct(It.IsAny<UpdateProductRequest>()))
                .ReturnsAsync(product);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new UpdateProductCommand(updateProduct), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            UpdateProductRequest updateProduct = new(1, "First", "Desc", 1, 1, 1, file);
            _service.Setup(s => s.UpdateProduct(It.IsAny<UpdateProductRequest>()))
                .ReturnsAsync((Product)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new UpdateProductCommand(updateProduct), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
