using App.Metrics;
using App.Metrics.Counter;
using Core.Contracts.Controllers.Orders;
using Core.Entities;
using Core.Mediator.Commands.Orders;
using Infrastructure.Mediator.Handlers.Orders;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Orders
{
    public sealed class UpdateOrderHandlerTests
    {
        private readonly Mock<IOrderService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly UpdateOrderHandler _handler;

        public UpdateOrderHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnOrder()
        {
            //Arrange
            UpdateOrderRequest updateOrder = new(1, "First", "a@a.co", "address", "CityFirst", "Country", "Zip", false, new List<UpdateCartLineRequest>());
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
            _service.Setup(s => s.UpdateOrder(It.IsAny<UpdateOrderRequest>()))
                .ReturnsAsync(order);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new UpdateOrderCommand(updateOrder), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Order>();
            result.Should().BeEquivalentTo(order);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            UpdateOrderRequest updateOrder = new(1, "First", "a@a.co", "address", "CityFirst", "Country", "Zip", false, new List<UpdateCartLineRequest>());
            _service.Setup(s => s.UpdateOrder(It.IsAny<UpdateOrderRequest>()))
                .ReturnsAsync((Order)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new UpdateOrderCommand(updateOrder), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
