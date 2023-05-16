﻿using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Mediator.Handlers.Ratings;
using Infrastructure.Services.Interfaces;
using Moq;
using System.Security.Claims;

namespace Infrastructure.Tests.Mediator.Handlers.Ratings
{
    public sealed class CreateRatingHandlerTests
    {
        private readonly Mock<IRatingService> _service;
        private readonly CreateRatingHandler _handler;

        public CreateRatingHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnRating()
        {
            //Arrange
            CreateRatingRequest createRating = new(1, 1, "First");

            Rating rating = new()
            {
                RatingId = Guid.NewGuid(),
                ProductId = 1,
                UserId = Guid.NewGuid().ToString(),
                Value = 1,
                Comment = "First",
            };
            _service.Setup(s => s.CreateRating(It.IsAny<CreateRatingRequest>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(rating);

            //Act
            var result = _handler.Handle(new CreateRatingCommand(createRating, new ClaimsPrincipal()), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Rating>();
            result.Should().BeEquivalentTo(rating);
        }
    }
}
