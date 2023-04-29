using Core.Contracts.Controllers.Categories;
using Core.Entities;
using Infrastructure.Mapster;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using Mapster;
using MapsterMapper;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.Tests.Services
{
    public sealed class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<ICacheService> _cacheService;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _unitOfWork = new();
            _mapper = GetMapper();
            _cacheService = new();
            _categoryService = new(_unitOfWork.Object, _mapper, _cacheService.Object); ;
        }


        #region GetCategories

        [Fact]
        public void GetCategories_WhenCalled_ReturnCategories()
        {
            //Arrange
            Category[] categories = new Category[]
            {
                new()
                {
                    CategoryId = 1, Name = "First", Products = new List<Product>()
                },
                new()
                {
                    CategoryId = 2, Name = "Second", Products = new List<Product>()
                },
                new()
                {
                    CategoryId = 3, Name = "Third", Products = new List<Product>()
                },
            };

            _unitOfWork.Setup(u => u.Category.Categories).Returns(categories.AsQueryable());

            //Act
            var result = _categoryService.GetCategories().ToArray();


            //Arrange
            result.Should().BeOfType<Category[]>();
            result.Should().BeEquivalentTo(categories);
        }

        #endregion


        #region GetCategory

        [Fact]
        public void GetCategory_WhenCache_ReturnCategory()
        {
            //Arrange
            Category category = new()
            {
                CategoryId = 1,
                Name = "First",
                Products = new List<Product>()
            };
            _cacheService.Setup(u => u.GetAsync<Category>(It.IsAny<string>())).ReturnsAsync(category);

            //Act
            var result = _categoryService.GetCategory(1).Result;

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(category);
        }

        [Fact]
        public void GetCategory_WhenNoCache_ReturnCategory()
        {
            //Arrange
            Category[] categories = new Category[]
            {
                new()
                {
                    CategoryId = 1, Name = "First", Products = new List<Product>()
                },
                new()
                {
                    CategoryId = 2, Name = "Second", Products = new List<Product>()
                },
                new()
                {
                    CategoryId = 3, Name = "Third", Products = new List<Product>()
                },
            };
            var mock = categories.AsQueryable().BuildMock();

            _cacheService.Setup(u => u.GetAsync<Category>(It.IsAny<string>())).ReturnsAsync((Category)null!);
            _unitOfWork.Setup(u => u.Category.Categories).Returns(mock);

            //Act
            var result = _categoryService.GetCategory(1).Result;

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(categories[0]);
        }

        #endregion


        #region CreateCategory

        [Fact]
        public void CreateCategory_WhenCalled_ReturnCategory()
        {
            //Arrange
            CreateCategoryRequest createCategory = new("First");
            Category Category = new()
            {
                CategoryId = 0,
                Name = "First",
                Products = null
            };
            _unitOfWork.Setup(u => u.Category.CreateCategoryAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            //Act
            var result = _categoryService.CreateCategory(createCategory).Result;

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(Category);
        }

        #endregion


        #region UpdateCategory

        [Fact]
        public void UpdateCategory_WhenCalled_ReturnCategory()
        {
            //Arrange
            UpdateCategoryRequest updateCategory = new(1, "First");

            Category[] categories = { new() { CategoryId = updateCategory.CategoryId, Name = updateCategory.Name } };

            var mock = categories.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Category.Categories).Returns(mock);

            //Act
            var result = _categoryService.UpdateCategory(updateCategory).Result;

            //Assert
            result.Should().BeOfType<Category>();
        }

        [Fact]
        public void UpdateCategory_WhenCalled_ReturnNull()
        {
            //Arrange
            UpdateCategoryRequest updateCategory = new(1, "First");
            Category[] categories = Array.Empty<Category>();
            var mock = categories.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Category.Categories).Returns(mock);

            //Act
            var result = _categoryService.UpdateCategory(updateCategory).Result;

            //Assert
            result.Should().BeNull();
        }

        #endregion


        #region DeleteCategory

        [Fact]
        public void DeleteCategory_WhenCalled_ReturnCategory()
        {
            //Arrange
            Category[] categories = new Category[]
            {
                new()
                {
                    CategoryId = 1, Name = "First", Products = new List<Product>()
                },
                new()
                {
                    CategoryId = 2, Name = "Second", Products = new List<Product>()
                },
                new()
                {
                    CategoryId = 3, Name = "Third", Products = new List<Product>()
                },
            };

            var mock = categories.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Category.Categories).Returns(mock);

            //Act
            var result = _categoryService.DeleteCategory(1).Result;

            //Assert
            result.Should().BeOfType<Category>();
        }

        [Fact]
        public void DeleteCategory_WhenCalled_ReturnNull()
        {
            //Arrange
            Category[] categories = Array.Empty<Category>();
            var mock = categories.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Category.Categories).Returns(mock);

            //Act
            var result = _categoryService.DeleteCategory(1).Result;

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
