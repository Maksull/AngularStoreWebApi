using Core.Contracts.Controllers.Suppliers;
using Core.Validators.Suppliers;

namespace Core.Tests.Validators.Suppliers
{
    public sealed class UpdateSupplierRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidUpdateSupplierRequest()
        {
            //Arrange
            UpdateSupplierRequest updateSupplierRequest = new(1, "Test", "City");
            UpdateSupplierRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateSupplierRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidUpdateSupplierRequest()
        {
            //Arrange
            UpdateSupplierRequest updateSupplierRequest = new(0, " ", " ");
            UpdateSupplierRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateSupplierRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "SupplierId");
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "City");
        }
    }
}
