using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Infrastructure.Mediator.Handlers.Auth;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Auth
{
    public sealed class LoginHandlerTests
    {
        private readonly Mock<IAuthService> _service;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnJwtResponse()
        {
            //Arrange
            LoginRequest loginRequest = new("First", "Password");
            JwtResponse jwtResponse = new("jwt", new());
            _service.Setup(s => s.Login(It.IsAny<LoginRequest>()))
                .ReturnsAsync(jwtResponse);

            //Act
            var result = _handler.Handle(new LoginCommand(loginRequest), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<JwtResponse>();
            result.Should().BeEquivalentTo(jwtResponse);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            LoginRequest loginRequest = new("First", "Password");
            JwtResponse jwtResponse = new("jwt", new());
            _service.Setup(s => s.Login(It.IsAny<LoginRequest>()))
                .ReturnsAsync((JwtResponse)null!);

            //Act
            var result = _handler.Handle(new LoginCommand(loginRequest), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
