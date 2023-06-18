using App.Metrics;
using App.Metrics.Counter;
using Core.Entities;
using Core.Mediator.Queries.Suppliers;
using Infrastructure.Mediator.Handlers.Suppliers;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Suppliers
{
    public sealed class GetSuppliersHandlerTests
    {
        private readonly Mock<ISupplierService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly GetSuppliersHandler _handler;

        public GetSuppliersHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnSuppliers()
        {
            //Arrange
            List<Supplier> suppliers = new()
            {
                new()
                {
                    SupplierId = 1,
                    Name = "First",
                    City = "City",
                },
                new()
                {
                    SupplierId = 2,
                    Name = "Second",
                    City = "City",
                }
            };
            _service.Setup(s => s.GetSuppliers())
                .Returns(suppliers);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new GetSuppliersQuery(), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<Supplier>>();
            result.Should().BeEquivalentTo(suppliers);
        }
    }
}
