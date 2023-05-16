using Core.Entities;
using Core.Mediator.Queries.Orders;
using Infrastructure.Mediator.Handlers.Orders;
using Infrastructure.Services.Interfaces;
using Moq;
using System.Security.Claims;

namespace Infrastructure.Tests.Mediator.Handlers.Orders
{
    public sealed class GetOrdersByUserIdHandlerTests
    {
        private readonly Mock<IOrderService> _service;
        private readonly GetOrdersByUserIdHandler _handler;

        public GetOrdersByUserIdHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnOrders()
        {
            //Arrange
            var orders = new List<Order>()
            {
                new()
                {
                    OrderId = 1, Name = "First", Email = "a@a.co", Address = "address", City = "CityFirst", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                new()
                {
                    OrderId = 2, Name = "Second", Email = "a@a.co", Address = "address", City = "CitySecond", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                new()
                {
                    OrderId = 3, Name = "First", Email = "a@a.co", Address = "address", City = "CityFirst", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
            };
            _service.Setup(s => s.GetOrdersByUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(orders);

            //Act
            var result = _handler.Handle(new GetOrdersByUserIdQuery(new ClaimsPrincipal()), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<Order>>();
            result.Should().BeEquivalentTo(orders);
        }
    }
}
