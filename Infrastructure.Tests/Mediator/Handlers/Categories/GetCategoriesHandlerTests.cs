using Core.Entities;
using Core.Mediator.Queries.Categories;
using Infrastructure.Mediator.Handlers.Categories;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Infrastructure.Tests.Mediator.Handlers.Categories
{
    public sealed class GetCategoriesHandlerTests
    {
        private readonly Mock<ICategoryService> _service;
        private readonly GetCategoriesHandler _handler;

        public GetCategoriesHandlerTests()
        {
            _service = new();
            _handler = new(_service.Object);
        }

        [Fact]
        public void Handle_WhenCalled_ReturnCategories()
        {
            //Arrange
            List<Category> categories = new()
            {
                new()
                {
                    CategoryId = 1,
                    Name = "First",
                },
                new()
                {
                    CategoryId = 2,
                    Name = "Second",
                }
            };
            _service.Setup(s => s.GetCategories())
                .Returns(categories);

            //Act
            var result = _handler.Handle(new GetCategoriesQuery(), CancellationToken.None).Result;

            //Assert
            result.Should().BeOfType<List<Category>>();
            result.Should().BeEquivalentTo(categories);
        }
    }
}
