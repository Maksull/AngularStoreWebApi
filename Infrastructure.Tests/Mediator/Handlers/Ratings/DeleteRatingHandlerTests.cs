using App.Metrics;
using App.Metrics.Counter;
using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Mediator.Handlers.Ratings;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Ratings
{
    public sealed class DeleteRatingHandlerTests
    {
        private readonly Mock<IRatingService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly DeleteRatingHandler _handler;

        public DeleteRatingHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnRating()
        {
            //Arrange
            Rating rating = new()
            {
                RatingId = Guid.NewGuid(),
                ProductId = 1,
                UserId = Guid.NewGuid().ToString(),
                Value = 1,
                Comment = "First",
            };
            _service.Setup(s => s.DeleteRating(It.IsAny<Guid>()))
                .ReturnsAsync(rating);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteRatingCommand(Guid.NewGuid()), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Rating>();
            result.Should().BeEquivalentTo(rating);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.DeleteRating(It.IsAny<Guid>()))
                .ReturnsAsync((Rating)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteRatingCommand(Guid.NewGuid()), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
