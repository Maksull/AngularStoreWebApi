using App.Metrics;
using App.Metrics.Counter;
using Core.Entities;
using Core.Mediator.Commands.Orders;
using Infrastructure.Mediator.Handlers.Orders;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Orders
{
    public sealed class DeleteOrderHandlerTests
    {
        private readonly Mock<IOrderService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly DeleteOrderHandler _handler;

        public DeleteOrderHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
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
            _service.Setup(s => s.DeleteOrder(It.IsAny<long>()))
                .ReturnsAsync(order);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteOrderCommand(1), CancellationToken.None).Result;

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

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteOrderCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
