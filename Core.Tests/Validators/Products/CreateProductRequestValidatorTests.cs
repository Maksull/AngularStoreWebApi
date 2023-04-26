using Core.Contracts.Controllers.Products;
using Core.Validators.Products;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Core.Tests.Validators.Products
{
    public sealed class CreateProductRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidCreateProductRequest()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            CreateProductRequest createProductRequest = new("First", "Desc", 1, 1, 1, file);
            CreateProductRequestValidator validator = new();

            //Act
            var result = validator.Validate(createProductRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidCreateProductRequest()
        {
            //Arrange
            CreateProductRequest createProductRequest = new(" ", " ", 0, 0, 0, null!);
            CreateProductRequestValidator validator = new();

            //Act
            var result = validator.Validate(createProductRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "Description");
            result.Errors.Should().Contain(e => e.PropertyName == "Price");
            result.Errors.Should().Contain(e => e.PropertyName == "CategoryId");
            result.Errors.Should().Contain(e => e.PropertyName == "SupplierId");
            result.Errors.Should().Contain(e => e.PropertyName == "Img");
        }
    }
}
