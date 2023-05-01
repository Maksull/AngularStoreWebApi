using Core.Entities;
using Core.Mediator.Queries.Products;
using Infrastructure.Mediator.Handlers.Products;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Products
{
    public sealed class GetProductByIdHandlerTests
    {
        private readonly Mock<IProductService> _service;
        private readonly GetProductsByIdHandler _handler;

        public GetProductByIdHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnProduct()
        {
            //Arrange
            Product product = new()
            {
                Name = "First",
            };
            _service.Setup(s => s.GetProduct(It.IsAny<long>()))
                .ReturnsAsync(product);

            //Act
            var result = _handler.Handle(new GetProductByIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.GetProduct(It.IsAny<long>()))
                .ReturnsAsync((Product)null!);

            //Act
            var result = _handler.Handle(new GetProductByIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
