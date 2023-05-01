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
        private readonly DeleteSupplierHandler _handler;

        public DeleteSupplierHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
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

            //Act
            var result = _handler.Handle(new DeleteSupplierCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
