using App.Metrics;
using App.Metrics.Counter;
using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Mediator.Handlers.Ratings;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Ratings
{
    public sealed class GetRatingsHandlerTests
    {
        private readonly Mock<IRatingService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly GetRatingsHandler _handler;

        public GetRatingsHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnRatings()
        {
            //Arrange
            List<Rating> ratings = new()
            {
                new()
                {
                    RatingId = Guid.NewGuid(),
                    ProductId = 1,
                    UserId = Guid.NewGuid().ToString(),
                    Value = 1,
                    Comment = "First",
                },
                 new()
                {
                    RatingId = Guid.NewGuid(),
                    ProductId = 2,
                    UserId = Guid.NewGuid().ToString(),
                    Value = 2,
                    Comment = "Second",
                },
            };
            _service.Setup(s => s.GetRatings())
                .Returns(ratings);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new GetRatingsQuery(), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<Rating>>();
            result.Should().BeEquivalentTo(ratings);
        }
    }
}
