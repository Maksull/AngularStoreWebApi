using Core.Mediator.Commands.Auth;
using Infrastructure.Mediator.Handlers.Auth;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Auth
{
    public sealed class ResetPasswordHandlerTests
    {
        private readonly Mock<IAuthService> _service;
        private readonly ResetPasswordHandler _handler;

        public ResetPasswordHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnTrue()
        {
            //Arrange
            ResetPasswordCommand resetPasswordCommand = new(Guid.NewGuid().ToString(), "username");
            _service.Setup(s => s.ResetPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            var result = _handler.Handle(resetPasswordCommand, CancellationToken.None).Result;

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Handle_WhenCalled_ReturnFalse()
        {
            //Arrange
            ResetPasswordCommand resetPasswordCommand = new(Guid.Empty.ToString(), "username");
            _service.Setup(s => s.ResetPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            //Act
            var result = _handler.Handle(resetPasswordCommand, CancellationToken.None).Result;

            //Assert
            result.Should().BeFalse();
        }
    }
}
