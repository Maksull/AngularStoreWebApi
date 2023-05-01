using Core.Contracts.Controllers.Products;
using Core.Validators.Products;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Core.Tests.Validators.Products
{
    public sealed class UpdateProductRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenCalled_ShouldNotHaveErrors_For_ValidUpdateProductRequest()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            UpdateProductRequest updateProductRequest = new(1, "First", "Desc", 1, 1, 1, file);
            UpdateProductRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateProductRequest);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldHaveErrors_For_InvalidUpdateProductRequest()
        {
            //Arrange
            UpdateProductRequest updateProductRequest = new(0, " ", " ", 0, 0, 0, null!);
            UpdateProductRequestValidator validator = new();

            //Act
            var result = validator.Validate(updateProductRequest);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.PropertyName == "Description");
            result.Errors.Should().Contain(e => e.PropertyName == "Price");
            result.Errors.Should().Contain(e => e.PropertyName == "CategoryId");
            result.Errors.Should().Contain(e => e.PropertyName == "SupplierId");
        }
    }
}
