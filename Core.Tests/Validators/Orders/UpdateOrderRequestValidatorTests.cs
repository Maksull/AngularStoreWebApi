using Core.Contracts.Controllers.Orders;
using Core.Validators.Orders;

namespace Core.Tests.Validators.Orders
{
    public sealed class UpdateOrderRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidUpdateOrderRequest()
        {
            //Arrange
            UpdateOrderRequest updateOrderRequest = new(1, "First", "a@a.co", "address", "CityFirst", "Country", "Zip", true, new List<UpdateCartLineRequest>() { new UpdateCartLineRequest(1, 1, 1, 1) });
            UpdateOrderRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateOrderRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidUpdateOrderRequest()
        {
            //Arrange
            UpdateOrderRequest updateOrderRequest = new(0, " ", " ", " ", "  ", "  ", "  ", default, new List<UpdateCartLineRequest>());
            UpdateOrderRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateOrderRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "OrderId");
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
            result.Errors.Should().Contain(e => e.PropertyName == "Address");
            result.Errors.Should().Contain(e => e.PropertyName == "City");
            result.Errors.Should().Contain(e => e.PropertyName == "Country");
            result.Errors.Should().Contain(e => e.PropertyName == "Zip");
            result.Errors.Should().Contain(e => e.PropertyName == "IsShipped");
            result.Errors.Should().Contain(e => e.PropertyName == "Lines");
        }
    }
}
