using Core.Entities;
using Core.Mediator.Queries.Products;
using Infrastructure.Mediator.Handlers.Products;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Products
{
    public sealed class GetProductsHandlerTests
    {
        private readonly Mock<IProductService> _service;
        private readonly GetProductsHandler _handler;

        public GetProductsHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnProducts()
        {
            //Arrange
            var products = new List<Product>()
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
            };
            _service.Setup(s => s.GetProducts())
                .Returns(products);

            //Act
            var result = _handler.Handle(new GetProductsQuery(), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<Product>>();
            result.Should().BeEquivalentTo(products);
        }
    }
}
