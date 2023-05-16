using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public sealed class RatingRepositoryTests
    {
        [Fact]
        public void Ratings_WhenCalled_ReturnRatings()
        {
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

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "Ratings")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Ratings.AddRange(ratings);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                RatingRepository ratingRepository = new(context);
                var result = (ratingRepository.Ratings.AsNoTracking()).ToArray();

                result.Should().BeOfType<Rating[]>();
                result.Should().BeEquivalentTo(ratings);
            }
        }

        [Fact]
        public async void CreateRatingAsync_WhenCalled_AddRating()
        {
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
            Rating rating = new()
            {
                RatingId = Guid.NewGuid(),
                ProductId = 3,
                UserId = Guid.NewGuid().ToString(),
                Value = 3,
                Comment = "Third",
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "CreateRating")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Ratings.AddRange(ratings);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                RatingRepository ratingRepository = new(context);
                var oldResult = (ratingRepository.Ratings.AsNoTracking()).ToArray();
                await ratingRepository.CreateRatingAsync(rating);
                var newResult = (ratingRepository.Ratings.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Rating[]>();
                oldResult.Should().BeEquivalentTo(ratings);
                newResult.Should().BeOfType<Rating[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(ratings.Length + 1);
            }
        }

        [Fact]
        public async void UpdateRatingAsync_WhenCalled_UpdateRating()
        {
            Rating rating = new()
            {
                RatingId = Guid.NewGuid(),
                ProductId = 3,
                UserId = Guid.NewGuid().ToString(),
                Value = 3,
                Comment = "Third",
            };
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
                rating,
            };


            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateRating")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Ratings.AddRange(ratings);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                RatingRepository ratingRepository = new(context);
                rating.Value = 1;
                rating.Comment = "NewComment";

                await ratingRepository.UpdateRatingAsync(rating);
                var result = (ratingRepository.Ratings.AsNoTracking()).ToArray();

                result.Should().BeOfType<Rating[]>();
                result.Should().HaveCount(ratings.Length);
                result[2].Should().BeEquivalentTo(rating);
            }
        }

        [Fact]
        public async void DeleteRatingAsync_WhenCalled_DeleteRating()
        {
            Rating rating = new()
            {
                RatingId = Guid.NewGuid(),
                ProductId = 3,
                UserId = Guid.NewGuid().ToString(),
                Value = 3,
                Comment = "Third",
            };
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
                rating,
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "DeleteRating")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Ratings.AddRange(ratings);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                RatingRepository ratingRepository = new(context);
                var oldResult = (ratingRepository.Ratings.AsNoTracking()).ToArray();
                await ratingRepository.DeleteRatingAsync(rating);
                var newResult = (ratingRepository.Ratings.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Rating[]>();
                oldResult.Should().BeEquivalentTo(ratings);
                newResult.Should().BeOfType<Rating[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(ratings.Length - 1);
            }
        }
    }
}
