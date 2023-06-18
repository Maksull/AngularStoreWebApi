using App.Metrics;
using App.Metrics.Counter;
using Core.Mediator.Commands.Auth;
using Infrastructure.Mediator.Handlers.Auth;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Auth
{
    public sealed class ResetPasswordHandlerTests
    {
        private readonly Mock<IAuthService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly ResetPasswordHandler _handler;

        public ResetPasswordHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnTrue()
        {
            //Arrange
            ResetPasswordCommand resetPasswordCommand = new(Guid.NewGuid().ToString(), "username");
            _service.Setup(s => s.ResetPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

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

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(resetPasswordCommand, CancellationToken.None).Result;

            //Assert
            result.Should().BeFalse();
        }
    }
}
