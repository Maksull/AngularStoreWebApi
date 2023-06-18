using App.Metrics;
using App.Metrics.Counter;
using Core.Contracts.Controllers.Auth;
using Core.Mediator.Queries.Auth;
using Infrastructure.Mediator.Handlers.Auth;
using Infrastructure.Services.Interfaces;
using Moq;
using System.Security.Claims;

namespace Infrastructure.Tests.Mediator.Handlers.Auth
{
    public sealed class GetUserDataHandlerTests
    {
        private readonly Mock<IAuthService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly GetUserDataHandler _handler;

        public GetUserDataHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnUserResponse()
        {
            //Arrange
            UserResponse userResponse = new("First", "Last", "Username", "your_email@coc.co", "+1");
            _service.Setup(s => s.GetUserData(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(userResponse);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new GetUserDataQuery(new ClaimsPrincipal()), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<UserResponse>();
            result.Should().BeEquivalentTo(userResponse);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.GetUserData(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync((UserResponse)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new GetUserDataQuery(new ClaimsPrincipal()), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
