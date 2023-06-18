using App.Metrics;
using App.Metrics.Counter;
using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Infrastructure.Mediator.Handlers.Auth;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Auth
{
    public sealed class RefreshHandlerTests
    {
        private readonly Mock<IAuthService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly RefreshHandler _handler;

        public RefreshHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnJwtResponse()
        {
            //Arrange
            RefreshTokenRequest refreshTokenRequest = new("Token", DateTime.Now);
            JwtResponse jwtResponse = new("jwt", new());
            _service.Setup(s => s.Refresh(It.IsAny<RefreshTokenRequest>()))
                .ReturnsAsync(jwtResponse);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new RefreshCommand(refreshTokenRequest), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<JwtResponse>();
            result.Should().BeEquivalentTo(jwtResponse);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            RefreshTokenRequest refreshTokenRequest = new("Token", DateTime.Now);
            JwtResponse jwtResponse = new("jwt", new());
            _service.Setup(s => s.Refresh(It.IsAny<RefreshTokenRequest>()))
                .ReturnsAsync((JwtResponse)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new RefreshCommand(refreshTokenRequest), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
