using Core.Contracts.Controllers.Auth;
using Core.Validators.Auth;

namespace Core.Tests.Validators.Auth
{
    public sealed class RefreshTokenRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidRefreshTokenRequest()
        {
            //Arrange
            RefreshTokenRequest refreshTokenRequest = new("UN", DateTime.Now);
            RefreshTokenRequestValidator validator = new();

            //Act
            var result = validator.Validate(refreshTokenRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidRefreshTokenRequest()
        {
            //Arrange
            RefreshTokenRequest refreshTokenRequest = new(" ", default);
            RefreshTokenRequestValidator validator = new();

            //Act
            var result = validator.Validate(refreshTokenRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Token");
            result.Errors.Should().Contain(e => e.PropertyName == "Expired");
        }
    }
}
