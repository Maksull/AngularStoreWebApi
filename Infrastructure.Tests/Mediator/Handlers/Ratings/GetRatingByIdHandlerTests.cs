using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Mediator.Handlers.Ratings;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Ratings
{
    public sealed class GetRatingByIdHandlerTests
    {
        private readonly Mock<IRatingService> _service;
        private readonly GetRatingByIdHandler _handler;

        public GetRatingByIdHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
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
            _service.Setup(s => s.GetRating(It.IsAny<Guid>()))
                .ReturnsAsync(rating);

            //Act
            var result = _handler.Handle(new GetRatingByIdQuery(Guid.NewGuid()), CancellationToken.None).Result;

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

            //Act
            var result = _handler.Handle(new GetRatingByIdQuery(Guid.NewGuid()), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
