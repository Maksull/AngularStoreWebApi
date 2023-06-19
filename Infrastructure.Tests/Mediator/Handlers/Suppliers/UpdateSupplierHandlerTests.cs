using App.Metrics;
using App.Metrics.Counter;
using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Mediator.Handlers.Suppliers;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Suppliers
{
    public sealed class UpdateSupplierHandlerTests
    {
        private readonly Mock<ISupplierService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly UpdateSupplierHandler _handler;

        public UpdateSupplierHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnSupplier()
        {
            //Arrange
            UpdateSupplierRequest updateSupplier = new(1, "First", "City");
            Supplier supplier = new()
            {
                SupplierId = 1,
                Name = "First",
                City = "City",
            };
            _service.Setup(s => s.UpdateSupplier(It.IsAny<UpdateSupplierRequest>()))
                .ReturnsAsync(supplier);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new UpdateSupplierCommand(updateSupplier), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
            result.Should().BeEquivalentTo(supplier);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            UpdateSupplierRequest updateSupplier = new(1, "First", "City");
            _service.Setup(s => s.UpdateSupplier(It.IsAny<UpdateSupplierRequest>()))
                .ReturnsAsync((Supplier)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new UpdateSupplierCommand(updateSupplier), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
