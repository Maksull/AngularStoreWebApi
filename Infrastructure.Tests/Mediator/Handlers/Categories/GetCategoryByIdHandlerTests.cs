using Core.Entities;
using Core.Mediator.Queries.Categories;
using Infrastructure.Mediator.Handlers.Categories;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Categories
{
    public sealed class GetCategoryByIdHandlerTests
    {
        private readonly Mock<ICategoryService> _service;
        private readonly GetCategoryByIdHandler _handler;

        public GetCategoryByIdHandlerTests()
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
                CategoryId = 1,
                Name = "First",
            };
            _service.Setup(s => s.GetCategory(It.IsAny<long>()))
                .ReturnsAsync(category);

            //Act
            var result = _handler.Handle(new GetCategoryByIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(category);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnNull()
        {
            //Arrange
            _service.Setup(s => s.GetCategory(It.IsAny<long>()))
                .ReturnsAsync((Category)null!);

            //Act
            var result = _handler.Handle(new GetCategoryByIdQuery(1), CancellationToken.None).Result;

            //Assert
            result.Should().BeNull();
        }
    }
}
