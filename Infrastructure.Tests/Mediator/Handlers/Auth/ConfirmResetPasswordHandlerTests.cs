using Core.Mediator.Commands.Auth;
using Infrastructure.Mediator.Handlers.Auth;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Auth
{
    public sealed class ConfirmResetPasswordHandlerTests
    {
        private readonly Mock<IAuthService> _service;
        private readonly ConfirmResetPasswordHandler _handler;

        public ConfirmResetPasswordHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnEmptyList()
        {
            //Arrange
            ConfirmResetPasswordCommand confirmResetPasswordCommand = new(Guid.NewGuid().ToString(), "token", "newPassword");
            _service.Setup(s => s.ConfirmResetPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<string>());

            //Act
            var result = _handler.Handle(confirmResetPasswordCommand, CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<string>>();
            result.Should().BeEmpty();
        }

        [Fact]
        public void Handle_WhenCalled_ReturnListWithErrors()
        {
            //Arrange
            List<string> errors = new()
            {
                "Test error",
            };
            ConfirmResetPasswordCommand confirmResetPasswordCommand = new(Guid.NewGuid().ToString(), "token", "newPassword");
            _service.Setup(s => s.ConfirmResetPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(errors);

            //Act
            var result = _handler.Handle(confirmResetPasswordCommand, CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<string>>();
            result.Should().BeEquivalentTo(errors);
        }
    }
}
