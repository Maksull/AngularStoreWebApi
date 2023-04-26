using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Mediator.Handlers.Suppliers;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Suppliers
{
    public sealed class CreateSupplierHandlerTests
    {
        private readonly Mock<ISupplierService> _service;
        private readonly CreateSupplierHandler _handler;

        public CreateSupplierHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnSupplier()
        {
            //Arrange
            CreateSupplierRequest createSupplier = new("First", "City");
            Supplier supplier = new()
            {
                Name = "First",
                City = "City",
            };
            _service.Setup(s => s.CreateSupplier(It.IsAny<CreateSupplierRequest>()))
                .ReturnsAsync(supplier);

            //Act
            var result = _handler.Handle(new CreateSupplierCommand(createSupplier), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
            result.Should().BeEquivalentTo(supplier);
        }
    }
}
