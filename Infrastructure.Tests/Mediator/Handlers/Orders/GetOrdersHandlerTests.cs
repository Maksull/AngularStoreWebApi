using App.Metrics;
using App.Metrics.Counter;
using Core.Entities;
using Core.Mediator.Queries.Orders;
using Infrastructure.Mediator.Handlers.Orders;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Orders
{
    public sealed class GetOrdersHandlerTests
    {
        private readonly Mock<IOrderService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly GetOrdersHandler _handler;

        public GetOrdersHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
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
            _service.Setup(s => s.GetOrders())
                .Returns(orders);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new GetOrdersQuery(), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<Order>>();
            result.Should().BeEquivalentTo(orders);
        }
    }
}
