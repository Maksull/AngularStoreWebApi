﻿using App.Metrics;
using App.Metrics.Counter;
using Core.Contracts.Controllers.Categories;
using Core.Entities;
using Core.Mediator.Commands.Categories;
using Infrastructure.Mediator.Handlers.Categories;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Categories
{
    public sealed class CreateCategoryHandlerTests
    {
        private readonly Mock<ICategoryService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly CreateCategoryHandler _handler;

        public CreateCategoryHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnCategory()
        {
            //Arrange
            CreateCategoryRequest createCategory = new("First");
            Category category = new()
            {
                Name = "First",
            };
            _service.Setup(s => s.CreateCategory(It.IsAny<CreateCategoryRequest>()))
                .ReturnsAsync(category);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new CreateCategoryCommand(createCategory), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(category);
        }
    }
}
