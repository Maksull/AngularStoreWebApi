using Core.Contracts.Controllers.Ratings;
using Core.Validators.Ratings;

namespace Core.Tests.Validators.Ratings
{
    public sealed class CreateRatingRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidCreateRatingRequest()
        {
            //Arrange
            CreateRatingRequest createRatingRequest = new(1, 1, "First");
            CreateRatingRequestValidator validator = new();

            //Act
            var result = validator.Validate(createRatingRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidCreateRatingRequest()
        {
            //Arrange
            CreateRatingRequest createRatingRequest = new(0, 6, null!);
            CreateRatingRequestValidator validator = new();

            //Act
            var result = validator.Validate(createRatingRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
            result.Errors.Should().Contain(e => e.PropertyName == "Value");
            result.Errors.Should().Contain(e => e.PropertyName == "Comment");
        }
    }
}
