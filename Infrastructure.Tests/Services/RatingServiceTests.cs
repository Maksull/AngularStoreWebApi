using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using Infrastructure.Mapster;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using Mapster;
using MapsterMapper;
using MockQueryable.Moq;
using Moq;
using System.Security.Claims;

namespace Infrastructure.Tests.Services
{
    public sealed class RatingServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<ICacheService> _cacheService;
        private readonly RatingService _ratingService;

        public RatingServiceTests()
        {
            _unitOfWork = new();
            _mapper = GetMapper();
            _cacheService = new();
            _ratingService = new(_unitOfWork.Object, _mapper, _cacheService.Object);
        }

        #region GetRatings

        [Fact]
        public void GetRatings_WhenCalled_ReturnOrders()
        {
            //Arrange
            Rating[] ratings =
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

            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(ratings.AsQueryable());

            //Act
            var result = _ratingService.GetRatings().ToArray();


            //Arrange
            result.Should().BeOfType<Rating[]>();
            result.Should().BeEquivalentTo(ratings);
        }

        #endregion


        #region GetRatingsByProductId

        [Fact]
        public void GetRatingsByProductId_WhenCalled_ReturnOrders()
        {
            //Arrange
            Rating[] ratings =
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

            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(ratings.AsQueryable());

            //Act
            var result = _ratingService.GetRatingsByProductId(1).ToArray();


            //Arrange
            result.Should().BeOfType<Rating[]>();
            result[0].Should().BeEquivalentTo(ratings[0]);
        }

        #endregion


        #region GetRatingsByUsertId

        [Fact]
        public void GetRatingsByUserId_WhenUserIdValid_ReturnOrders()
        {
            //Arrange
            string userId = Guid.NewGuid().ToString();
            Rating[] ratings =
            {
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 1, UserId = userId, Value = 1, Comment = "First",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 2, UserId = userId, Value = 2, Comment = "Second",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 3, UserId = userId, Value = 3, Comment = "Third",
                },
            };
            var userRequest = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(ratings.AsQueryable());

            //Act
            var result = _ratingService.GetRatingsByUserId(userRequest).ToArray();


            //Arrange
            result.Should().BeOfType<Rating[]>();
            result.Should().BeEquivalentTo(ratings);
        }

        [Fact]
        public void GetRatingsByUserId_WhenUserIdInvalid_ReturnOrders()
        {
            //Arrange
            string userId = Guid.NewGuid().ToString();
            Rating[] ratings =
            {
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 1, UserId = userId, Value = 1, Comment = "First",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 2, UserId = userId, Value = 2, Comment = "Second",
                },
                new()
                {
                    RatingId = Guid.NewGuid(), ProductId = 3, UserId = userId, Value = 3, Comment = "Third",
                },
            };

            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(ratings.AsQueryable());

            //Act
            var result = _ratingService.GetRatingsByUserId(new ClaimsPrincipal()).ToArray();


            //Arrange
            result.Should().BeOfType<Rating[]>();
            result.Should().BeEmpty();
        }

        #endregion


        #region GetRating

        [Fact]
        public void GetRating_WhenCache_ReturnRating()
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
            _cacheService.Setup(u => u.GetAsync<Rating>(It.IsAny<string>())).ReturnsAsync(rating);

            //Act
            var result = _ratingService.GetRating(rating.RatingId).Result;

            //Assert
            result.Should().BeOfType<Rating>();
            result.Should().BeEquivalentTo(rating);
        }

        [Fact]
        public void GetRating_WhenNoCache_ReturnRating()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Rating[] ratings =
            {
                new()
                {
                    RatingId = id, ProductId = 1, UserId = Guid.NewGuid().ToString(), Value = 1, Comment = "First",
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
            var mock = ratings.AsQueryable().BuildMock();

            _cacheService.Setup(u => u.GetAsync<Rating>(It.IsAny<string>())).ReturnsAsync((Rating)null!);
            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(mock);

            //Act
            var result = _ratingService.GetRating(id).Result;

            //Assert
            result.Should().BeOfType<Rating>();
            result.Should().BeEquivalentTo(ratings[0]);
        }

        #endregion


        #region CreateRating

        [Fact]
        public void CreateRating_WhenCalled_ReturnRating()
        {
            //Arrange
            CreateRatingRequest createRating = new(1, 1, "First");
            string userId = Guid.NewGuid().ToString();
            Rating rating = new()
            {
                ProductId = 1,
                UserId = userId,
                Value = 1,
                Comment = "First",
            };
            var userRequest = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            _unitOfWork.Setup(u => u.Rating.CreateRatingAsync(It.IsAny<Rating>())).Returns(Task.CompletedTask);

            //Act
            var result = _ratingService.CreateRating(createRating, userRequest).Result;

            //Assert
            result.Should().BeOfType<Rating>();
            result.Should().BeEquivalentTo(rating);
        }

        #endregion


        #region UpdateRating

        [Fact]
        public void UpdateRating_WhenCalled_ReturnRating()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();
            UpdateRatingRequest updateRating = new(id, 1, 1, "First");
            Rating[] ratings =
            {
                new()
                {
                    RatingId = id, ProductId = 1, UserId = userId, Value = 1, Comment = "First",
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
            var userRequest = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            var mock = ratings.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(mock);

            //Act
            var result = _ratingService.UpdateRating(updateRating, userRequest).Result;

            //Assert
            result.Should().BeOfType<Rating>();
        }

        [Fact]
        public void UpdateRating_WhenCalled_ReturnNull()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();
            UpdateRatingRequest updateRating = new(id, 1, 1, "First");
            Rating[] ratings = Array.Empty<Rating>();
            var userRequest = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            var mock = ratings.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(mock);

            //Act
            var result = _ratingService.UpdateRating(updateRating, userRequest).Result;

            //Assert
            result.Should().BeNull();
        }

        #endregion


        #region DeleteRating

        [Fact]
        public void DeleteRating_WhenCalled_ReturnRating()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Rating[] ratings =
            {
                new()
                {
                    RatingId = id, ProductId = 1, UserId = Guid.NewGuid().ToString(), Value = 1, Comment = "First",
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

            var mock = ratings.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(mock);

            //Act
            var result = _ratingService.DeleteRating(id).Result;

            //Assert
            result.Should().BeOfType<Rating>();
        }

        [Fact]
        public void DeleteRating_WhenCalled_ReturnNull()
        {
            //Arrange
            Rating[] ratings = Array.Empty<Rating>();
            var mock = ratings.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Rating.Ratings).Returns(mock);

            //Act
            var result = _ratingService.DeleteRating(Guid.NewGuid()).Result;

            //Assert
            result.Should().BeNull();
        }

        #endregion


        private static Mapper GetMapper()
        {
            TypeAdapterConfig config = new();
            config.Apply(new MapsterRegister());

            return new Mapper(config);
        }
    }
}
