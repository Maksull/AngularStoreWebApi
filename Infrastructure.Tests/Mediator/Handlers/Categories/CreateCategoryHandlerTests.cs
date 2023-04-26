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
        private readonly CreateCategoryHandler _handler;

        public CreateCategoryHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
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

            //Act
            var result = _handler.Handle(new CreateCategoryCommand(createCategory), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(category);
        }
    }
}
