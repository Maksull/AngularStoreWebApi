using Core.Contracts.Controllers.Orders;
using Core.Validators.Orders;

namespace Core.Tests.Validators.Orders
{
    public sealed class CreateOrderRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidCreateOrderRequest()
        {
            //Arrange
            CreateOrderRequest createOrderRequest = new("First", "a@a.co", "address", "CityFirst", "Country", "Zip", new List<CreateCartLineRequest>() { new CreateCartLineRequest(1, 1) });
            CreateOrderRequestValidator validator = new();

            //Act
            var result = validator.Validate(createOrderRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidCreateOrderRequest()
        {
            //Arrange
            CreateOrderRequest createOrderRequest = new(" ", " ", " ", "  ", "  ", "  ", new List<CreateCartLineRequest>());
            CreateOrderRequestValidator validator = new();

            //Act
            var result = validator.Validate(createOrderRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
            result.Errors.Should().Contain(e => e.PropertyName == "Address");
            result.Errors.Should().Contain(e => e.PropertyName == "City");
            result.Errors.Should().Contain(e => e.PropertyName == "Country");
            result.Errors.Should().Contain(e => e.PropertyName == "Zip");
            result.Errors.Should().Contain(e => e.PropertyName == "Lines");
        }
    }
}
