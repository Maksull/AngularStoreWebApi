using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Mediator.Handlers.Ratings;
using Infrastructure.Services.Interfaces;
using Moq;
using System.Security.Claims;

namespace Infrastructure.Tests.Mediator.Handlers.Ratings
{
    public sealed class GetRatingsByUserIdHandlerTests
    {
        private readonly Mock<IRatingService> _service;
        private readonly GetRatingsByUserIdHandler _handler;

        public GetRatingsByUserIdHandlerTests()
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
            _service.Setup(s => s.GetRatingsByUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(ratings);

            //Act
            var result = _handler.Handle(new GetRatingsByUserIdQuery(new ClaimsPrincipal()), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<Rating>>();
            result.Should().BeEquivalentTo(ratings);
        }
    }
}
