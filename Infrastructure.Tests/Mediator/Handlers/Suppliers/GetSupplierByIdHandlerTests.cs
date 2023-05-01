using Core.Entities;
using Core.Mediator.Queries.Suppliers;
using Infrastructure.Mediator.Handlers.Suppliers;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Suppliers
{
    public sealed class GetSupplierByIdHandlerTests
    {
        private readonly Mock<ISupplierService> _service;
        private readonly GetSupplierByIdHandler _handler;

        public GetSupplierByIdHandlerTests()
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
                SupplierId = 1,
                Name = "First",
                City = "City",
            };
            _service.Setup(s => s.GetSupplier(It.IsAny<long>()))
                .ReturnsAsync(supplier);

            //Act
            var result = _handler.Handle(new GetSupplierByIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
            result.Should().BeEquivalentTo(supplier);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.GetSupplier(It.IsAny<long>()))
                .ReturnsAsync((Supplier)null!);

            //Act
            var result = _handler.Handle(new GetSupplierByIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
