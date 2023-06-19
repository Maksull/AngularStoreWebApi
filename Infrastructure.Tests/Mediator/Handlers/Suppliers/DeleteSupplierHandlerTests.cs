using App.Metrics;
using App.Metrics.Counter;
using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Mediator.Handlers.Suppliers;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Suppliers
{
    public sealed class DeleteSupplierHandlerTests
    {
        private readonly Mock<ISupplierService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly DeleteSupplierHandler _handler;

        public DeleteSupplierHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnSupplier()
        {
            //Arrange
            Supplier supplier = new()
            {
                Name = "First",
                City = "City",
            };
            _service.Setup(s => s.DeleteSupplier(It.IsAny<long>()))
                .ReturnsAsync(supplier);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteSupplierCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
            result.Should().BeEquivalentTo(supplier);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.DeleteSupplier(It.IsAny<long>()))
                .ReturnsAsync((Supplier)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteSupplierCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
