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
        private readonly DeleteCategoryHandler _handler;

        public DeleteCategoryHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
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

            //Act
            var result = _handler.Handle(new DeleteCategoryCommand(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
