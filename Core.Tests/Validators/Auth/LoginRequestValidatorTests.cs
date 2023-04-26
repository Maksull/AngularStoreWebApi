using Core.Contracts.Controllers.Auth;
using Core.Validators.Auth;

namespace Core.Tests.Validators.Auth
{
    public sealed class LoginRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidLoginRequest()
        {
            //Arrange
            LoginRequest loginRequest = new("UN", "Secret123$");
            LoginRequestValidator validator = new();

            //Act
            var result = validator.Validate(loginRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidLoginRequest()
        {
            //Arrange
            LoginRequest loginRequest = new(" ", "Secret");
            LoginRequestValidator validator = new();

            //Act
            var result = validator.Validate(loginRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }
    }
}
