using App.Metrics;
using App.Metrics.Counter;
using Core.Entities;
using Core.Mediator.Commands.Categories;
using Infrastructure.Mediator.Handlers.Categories;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Categories
{
    public sealed class DeleteCategoryHandlerTests
    {
        private readonly Mock<ICategoryService> _service;
        private readonly Mock<IMetrics> _metrics;
        private readonly DeleteCategoryHandler _handler;

        public DeleteCategoryHandlerTests()
        {
            _service = new();
            _metrics = new();
            _handler = new(_service.Object, _metrics.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnCategory()
        {
            //Arrange
            Category category = new()
            {
                Name = "First",
            };
            _service.Setup(s => s.DeleteCategory(It.IsAny<long>()))
                .ReturnsAsync(category);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteCategoryCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(category);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.DeleteCategory(It.IsAny<long>()))
                .ReturnsAsync((Category)null!);

            var counterMock = new Mock<IMeasureCounterMetrics>();
            _metrics.Setup(m => m.Measure.Counter).Returns(counterMock.Object);

            //Act
            var result = _handler.Handle(new DeleteCategoryCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
