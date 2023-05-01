using Core.Contracts.Controllers.Auth;
using Core.Validators.Auth;

namespace Core.Tests.Validators.Auth
{
    public sealed class RegisterRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidRegisterRequest()
        {
            //Arrange
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email@co.co", "Secret123$", "Secret123$");
            RegisterRequestValidator validator = new();

            //Act
            var result = validator.Validate(registerRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidRegisterRequest()
        {
            //Arrange
            RegisterRequest registerRequest = new(" ", " ", " ", "Email", "S", "A");
            RegisterRequestValidator validator = new();

            //Act
            var result = validator.Validate(registerRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
            result.Errors.Should().Contain(e => e.PropertyName == "LastName");
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
            result.Errors.Should().Contain(e => e.PropertyName == "ConfirmPassword");
        }
    }
}
