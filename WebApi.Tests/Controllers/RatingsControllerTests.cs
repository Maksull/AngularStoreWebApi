using Core.Contracts.Controllers.Orders;
using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Core.Mediator.Queries.Ratings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers
{
    public sealed class RatingsControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly RatingsController _controller;

        public RatingsControllerTests()
        {
            _mediator = new();
            _controller = new(_mediator.Object);
        }

        #region GetRatings

        [Fact]
        public void GetRatings_WhenCalled_ReturnOk()
        {
            //Arrange
            var ratings = new List<Rating>()
            {
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 1, UserId = Guid.NewGuid().ToString(), Value = 1, Comment = "First",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 2, UserId = Guid.NewGuid().ToString(), Value = 2, Comment = "Second",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 3, UserId = Guid.NewGuid().ToString(), Value = 3, Comment = "Third",
                },
            };
            _mediator.Setup(m => m.Send(It.IsAny<GetRatingsQuery>(), default))
                .ReturnsAsync(ratings);

            //Act
            var response = (_controller.GetRatings().Result as OkObjectResult)!;
            var result = response.Value as List<Rating>;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<List<Rating>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(ratings);
        }

        [Fact]
        public void GetRatings_WhenCalled_ReturnNotFound()
        {
            //Arrange
            var ratings = new List<Rating>();
            _mediator.Setup(m => m.Send(It.IsAny<GetRatingsQuery>(), default))
                .ReturnsAsync(ratings);


            //Act
            var response = _controller.GetRatings().Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region GetRatingsByProductId

        [Fact]
        public void GetRatingsByProductId_WhenCalled_ReturnOk()
        {
            //Arrange
            var ratings = new List<Rating>()
            {
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 1, UserId = Guid.NewGuid().ToString(), Value = 1, Comment = "First",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 2, UserId = Guid.NewGuid().ToString(), Value = 2, Comment = "Second",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 3, UserId = Guid.NewGuid().ToString(), Value = 3, Comment = "Third",
                },
            };
            _mediator.Setup(m => m.Send(It.IsAny<GetRatingsByProductIdQuery>(), default))
                .ReturnsAsync(ratings);

            //Act
            var response = (_controller.GetRatingsByProductId(1).Result as OkObjectResult)!;
            var result = response.Value as List<Rating>;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<List<Rating>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(ratings);
        }

        [Fact]
        public void GetRatingsByProductId_WhenCalled_ReturnNotFound()
        {
            //Arrange
            var orders = new List<Rating>();
            _mediator.Setup(m => m.Send(It.IsAny<GetRatingsByProductIdQuery>(), default))
                .ReturnsAsync(orders);


            //Act
            var response = _controller.GetRatingsByProductId(1).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region GetRatingsByUserId

        [Fact]
        public void GetRatingsByUserId_WhenCalled_ReturnOk()
        {
            //Arrange
            var ratings = new List<Rating>()
            {
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 1, UserId = Guid.NewGuid().ToString(), Value = 1, Comment = "First",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 2, UserId = Guid.NewGuid().ToString(), Value = 2, Comment = "Second",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 3, UserId = Guid.NewGuid().ToString(), Value = 3, Comment = "Third",
                },
            };
            _mediator.Setup(m => m.Send(It.IsAny<GetRatingsByUserIdQuery>(), default))
                .ReturnsAsync(ratings);

            //Act
            var response = (_controller.GetRatingsByUserId().Result as OkObjectResult)!;
            var result = response.Value as List<Rating>;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<List<Rating>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(ratings);
        }

        [Fact]
        public void GetRatingsByUserId_WhenCalled_ReturnNotFound()
        {
            //Arrange
            var orders = new List<Rating>();
            _mediator.Setup(m => m.Send(It.IsAny<GetRatingsByUserIdQuery>(), default))
                .ReturnsAsync(orders);


            //Act
            var response = _controller.GetRatingsByUserId().Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region GetRating

        [Fact]
        public void GetRating_WhenCalled_ReturnOk()
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
            _mediator.Setup(m => m.Send(It.IsAny<GetRatingByIdQuery>(), default))
                .ReturnsAsync(rating);

            //Act
            var response = (_controller.GetRating(Guid.NewGuid()).Result as OkObjectResult)!;
            var result = response.Value as Rating;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<Rating>();
            result.Should().BeEquivalentTo(rating);
        }

        [Fact]
        public void GetRating_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetRatingByIdQuery>(), default))
                .ReturnsAsync((Rating)null!);

            //Act
            var response = _controller.GetRating(Guid.NewGuid()).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region CreateRating

        [Fact]
        public void CreateRating_WhenCalled_ReturnOk()
        {
            //Arrange
            CreateRatingRequest createRating = new(1, 1, "First");

            _mediator.Setup(m => m.Send(It.IsAny<CreateRatingCommand>(), default))
                .ReturnsAsync(new Rating
                {
                    RatingId = Guid.NewGuid(),
                    ProductId = 1,
                    UserId = Guid.NewGuid().ToString(),
                    Value = 1,
                    Comment = "First",
                });

            //Act
            var response = _controller.CreateRating(createRating).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Rating>();
        }

        #endregion

        #region UpdateRating

        [Fact]
        public void UpdateRating_WhenCalled_ReturnOk()
        {
            //Arrange
            UpdateRatingRequest updateRating = new(Guid.NewGuid(), 1, 1, "First");

            _mediator.Setup(m => m.Send(It.IsAny<UpdateRatingCommand>(), default))
                .ReturnsAsync(new Rating
                {
                    RatingId = Guid.NewGuid(),
                    ProductId = 1,
                    UserId = Guid.NewGuid().ToString(),
                    Value = 1,
                    Comment = "First",
                });

            //Act
            var response = _controller.UpdateRating(updateRating).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Rating>();
        }

        [Fact]
        public void UpdateRating_WhenCalled_ReturnNotFound()
        {
            //Arrange
            UpdateRatingRequest updateRating = new(Guid.NewGuid(), 1, 1, "First");

            _mediator.Setup(m => m.Send(It.IsAny<UpdateRatingCommand>(), default))
                .ReturnsAsync((Rating)null!);

            //Act
            var response = _controller.UpdateRating(updateRating).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region DeleteRating

        [Fact]
        public void DeleteRating_WhenCalled_ReturnOk()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteRatingCommand>(), default))
                .ReturnsAsync(new Rating
                {
                    RatingId = Guid.NewGuid(),
                    ProductId = 1,
                    UserId = Guid.NewGuid().ToString(),
                    Value = 1,
                    Comment = "First",
                });

            //Act
            var response = _controller.DeleteRating(Guid.NewGuid()).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Rating>();
        }

        [Fact]
        public void DeleteRating_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteRatingCommand>(), default))
                .ReturnsAsync((Rating)null!);

            //Act
            var response = _controller.DeleteRating(Guid.NewGuid()).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion
    }
}
