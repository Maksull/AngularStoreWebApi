using Core.Contracts.Controllers.Categories;
using Core.Validators.Categories;

namespace Core.Tests.Validators.Categories
{
    public sealed class UpdateCategoryRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidUpdateCategoryRequest()
        {
            //Arrange
            UpdateCategoryRequest updateCategoryRequest = new(1, "Test");
            UpdateCategoryRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateCategoryRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidUpdateCategoryRequest()
        {
            //Arrange
            UpdateCategoryRequest updateCategoryRequest = new(0, " ");
            UpdateCategoryRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateCategoryRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CategoryId");
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }
    }
}
