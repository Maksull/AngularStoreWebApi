using Core.Contracts.Controllers.Suppliers;
using Core.Validators.Suppliers;

namespace Core.Tests.Validators.Suppliers
{
    public sealed class CreateSupplierRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidCreateSupplierRequest()
        {
            //Arrange
            CreateSupplierRequest createSupplierRequest = new("Test", "City");
            CreateSupplierRequestValidator validator = new();

            //Act
            var result = validator.Validate(createSupplierRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidCreateSupplierRequest()
        {
            //Arrange
            CreateSupplierRequest createSupplierRequest = new(" ", " ");
            CreateSupplierRequestValidator validator = new();

            //Act
            var result = validator.Validate(createSupplierRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "City");
        }
    }
}
