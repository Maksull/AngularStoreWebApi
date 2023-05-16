using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Mediator.Handlers.Ratings;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Ratings
{
    public sealed class GetRatingByProductIdHandlerTests
    {
        private readonly Mock<IRatingService> _service;
        private readonly GetRatingsByProductIdHandler _handler;

        public GetRatingByProductIdHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
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
            _service.Setup(s => s.GetRatingsByProductId(It.IsAny<long>()))
                .Returns(ratings);

            //Act
            var result = _handler.Handle(new GetRatingsByProductIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<Rating>>();
            result.Should().BeEquivalentTo(ratings);
        }
    }
}
