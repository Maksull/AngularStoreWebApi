﻿using App.Metrics;
using App.Metrics.Counter;
using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Mediator.Handlers.Ratings;
using Infrastructure.Services.Interfaces;
using Moq;
using System.Security.Claims;

namespace Infrastructure.Tests.Mediator.Handlers.Ratings
{
    public sealed class UpdateRatingHandlerTests
    {
        private readonly Mock<IRatingService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly UpdateRatingHandler _handler;

        public UpdateRatingHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnRating()
        {
            //Arrange
            UpdateRatingRequest updateRating = new(Guid.NewGuid(), 1, 1, "NewComment");
            Rating rating = new()
            {
                RatingId = Guid.NewGuid(),
                ProductId = 1,
                UserId = Guid.NewGuid().ToString(),
                Value = 1,
                Comment = "First",
            };
            _service.Setup(s => s.UpdateRating(It.IsAny<UpdateRatingRequest>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(rating);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new UpdateRatingCommand(updateRating, new ClaimsPrincipal()), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Rating>();
            result.Should().BeEquivalentTo(rating);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            UpdateRatingRequest updateRating = new(Guid.NewGuid(), 1, 1, "NewComment");
            _service.Setup(s => s.UpdateRating(It.IsAny<UpdateRatingRequest>(), It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync((Rating)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new UpdateRatingCommand(updateRating, new ClaimsPrincipal()), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
