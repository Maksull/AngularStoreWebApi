using Core.Contracts.Controllers.Orders;
using Core.Entities;
using Core.Mediator.Commands.Orders;
using Infrastructure.Mediator.Handlers.Orders;
using Infrastructure.Services.Interfaces;
using Moq;
using System.Security.Claims;

namespace Infrastructure.Tests.Mediator.Handlers.Orders
{
    public sealed class CreateOrderHandlerTests
    {
        private readonly Mock<IOrderService> _service;
        private readonly CreateOrderHandler _handler;

        public CreateOrderHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnOrder()
        {
            //Arrange
            CreateOrderRequest createOrder = new("First", "a@a.co", "address", "CityFirst", "Country", "Zip", new List<CreateCartLineRequest>());

            Order order = new()
            {
                OrderId = 1,
                Name = "First",
                Email = "a@a.co",
                Address = "address",
                City = "CityFirst",
                Country = "Country",
                Zip = "Zip",
                Lines = new List<CartLine>()
            };
            _service.Setup(s => s.CreateOrder(It.IsAny<CreateOrderRequest>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(order);

            //Act
            var result = _handler.Handle(new CreateOrderCommand(createOrder, new ClaimsPrincipal()), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Order>();
            result.Should().BeEquivalentTo(order);
        }
    }
}
