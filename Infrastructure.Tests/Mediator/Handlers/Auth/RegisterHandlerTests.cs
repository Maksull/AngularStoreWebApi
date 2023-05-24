using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Infrastructure.Mediator.Handlers.Auth;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Auth
{
    public sealed class RegisterHandlerTests
    {
        private readonly Mock<IAuthService> _service;
        private readonly RegisterHandler _handler;

        public RegisterHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnTrue()
        {
            //Arrange
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email","PhoneNumber","Password", "Password");
            List<string> errors = new();

            _service.Setup(s => s.Register(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(errors);

            //Act
            var result = _handler.Handle(new RegisterCommand(registerRequest), CancellationToken.None).Result;

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Handle_WhenCalled_ReturnFalse()
        {
            //Arrange
            RegisterRequest registerRequest = new("First", "Second", "Name", "Email", "PhoneNumber", "Password", "Password");
            List<string> errors = new()
            {
                "error"
            };

            _service.Setup(s => s.Register(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(errors);

            //Act
            var result = _handler.Handle(new RegisterCommand(registerRequest), CancellationToken.None).Result;

            //Assert
            result.Should().BeEquivalentTo(errors);
        }
    }
}
