using App.Metrics;
using App.Metrics.Counter;
using Core.Entities;
using Core.Mediator.Commands.Products;
using Infrastructure.Mediator.Handlers.Products;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Products
{
    public sealed class DeleteProductHandlerTests
    {
        private readonly Mock<IProductService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnProduct()
        {
            //Arrange
            Product product = new()
            {
                Name = "First",
            };
            _service.Setup(s => s.DeleteProduct(It.IsAny<long>()))
                .ReturnsAsync(product);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteProductCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.DeleteProduct(It.IsAny<long>()))
                .ReturnsAsync((Product)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteProductCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
