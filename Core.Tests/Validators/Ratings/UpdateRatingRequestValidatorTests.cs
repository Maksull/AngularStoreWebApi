using Core.Contracts.Controllers.Ratings;
using Core.Validators.Ratings;

namespace Core.Tests.Validators.Ratings
{
    public sealed class UpdateRatingRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidUpdateRatingRequest()
        {
            //Arrange
            UpdateRatingRequest updateRatingRequest = new(Guid.NewGuid(), 1, 1, "First");
            UpdateRatingRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateRatingRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidUpdateOrderRequest()
        {
            //Arrange
            UpdateRatingRequest updateRatingRequest = new(Guid.Empty, 0, 6, null!);
            UpdateRatingRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateRatingRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "RatingId");
            result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
            result.Errors.Should().Contain(e => e.PropertyName == "Value");
            result.Errors.Should().Contain(e => e.PropertyName == "Comment");
        }
    }
}
