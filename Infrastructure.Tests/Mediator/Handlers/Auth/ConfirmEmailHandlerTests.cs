using Core.Mediator.Commands.Auth;
using Infrastructure.Mediator.Handlers.Auth;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Auth
{
    public sealed class ConfirmEmailHandlerTests
    {
        private readonly Mock<IAuthService> _service;
        private readonly ConfirmEmailHandler _handler;

        public ConfirmEmailHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnTrue()
        {
            //Arrange
            ConfirmEmailCommand confirmEmailCommand = new(Guid.NewGuid().ToString(), "Token");
            _service.Setup(s => s.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            var result = _handler.Handle(confirmEmailCommand, CancellationToken.None).Result;

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Handle_WhenCalled_ReturnFalse()
        {
            //Arrange
            ConfirmEmailCommand confirmEmailCommand = new(Guid.Empty.ToString(), "Token");
            _service.Setup(s => s.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            //Act
            var result = _handler.Handle(confirmEmailCommand, CancellationToken.None).Result;

            //Assert
            result.Should().BeFalse();
        }
    }
}
