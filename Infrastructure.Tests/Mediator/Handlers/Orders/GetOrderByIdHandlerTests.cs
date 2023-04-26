using Core.Entities;
using Core.Mediator.Queries.Orders;
using Infrastructure.Mediator.Handlers.Orders;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Orders
{
    public sealed class GetOrderByIdHandlerTests
    {
        private readonly Mock<IOrderService> _service;
        private readonly GetOrderByIdHandler _handler;

        public GetOrderByIdHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnOrder()
        {
            //Arrange
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
            _service.Setup(s => s.GetOrder(It.IsAny<long>()))
                .ReturnsAsync(order);

            //Act
            var result = _handler.Handle(new GetOrderByIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Order>();
            result.Should().BeEquivalentTo(order);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.DeleteOrder(It.IsAny<long>()))
                .ReturnsAsync((Order)null!);

            //Act
            var result = _handler.Handle(new GetOrderByIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
