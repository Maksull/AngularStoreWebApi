using Core.Contracts.Controllers.Categories;
using Core.Validators.Categories;

namespace Core.Tests.Validators.Categories
{
    public sealed class CreateCategoryRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidCreateCategoryRequest()
        {
            //Arrange
            CreateCategoryRequest createCategoryRequest = new("Test");
            CreateCategoryRequestValidator validator = new();

            //Act
            var result = validator.Validate(createCategoryRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidCreateCategoryRequest()
        {
            //Arrange
            CreateCategoryRequest createCategoryRequest = new(" ");
            CreateCategoryRequestValidator validator = new();

            //Act
            var result = validator.Validate(createCategoryRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }
    }
}
