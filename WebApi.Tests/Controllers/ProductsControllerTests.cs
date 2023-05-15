using Core.Contracts.Controllers.Products;
using Core.Entities;
using Core.Mediator.Commands.Products;
using Core.Mediator.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers
{
    public sealed class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediator = new();
            _controller = new(_mediator.Object);
        }

        #region GetProducts

        [Fact]
        public void GetProducts_WhenCalled_ReturnOk()
        {
            //Arrange
            var products = new List<Product>()
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
            };
            _mediator.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), default))
                .ReturnsAsync(products);

            //Act
            var response = (_controller.GetProducts().Result as OkObjectResult)!;
            var result = response.Value as List<Product>;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<List<Product>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public void GetProducts_WhenCalled_ReturnNotFound()
        {
            //Arrange
            var products = new List<Product>();
            _mediator.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), default))
                .ReturnsAsync(products);


            //Act
            var response = _controller.GetProducts().Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region GetProduct

        [Fact]
        public void GetProduct_WhenCalled_ReturnOk()
        {
            //Arrange
            Product product = new()
            {
                ProductId = 1,
                Name = "First",
                Description = "Desc",
                Price = 1,
                CategoryId = 1,
                SupplierId = 1
            };
            _mediator.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
                .ReturnsAsync(product);

            //Act
            var response = (_controller.GetProduct(1).Result as OkObjectResult)!;
            var result = response.Value as Product;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void GetProduct_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
                .ReturnsAsync((Product)null!);

            //Act
            var response = _controller.GetProduct(1).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region CreateProduct

        [Fact]
        public void CreateProduct_WhenCalled_ReturnOk()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            CreateProductRequest createProduct = new("First", "Desc", 1, 1, 1, file);

            _mediator.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default))
            .ReturnsAsync(new Product
            {
                ProductId = 1,
                Name = "First",
                Description = "Desc",
                Price = 1,
                CategoryId = 1,
                SupplierId = 1
            });

            //Act
            var response = _controller.CreateProduct(createProduct).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Product>();
        }

        #endregion

        #region UpdateProduct

        [Fact]
        public void UpdateProduct_WhenCalled_ReturnOk()
        {
            //Arrange
            UpdateProductRequest updateProduct = new(1, "First", "Desc", 1, 1, 1, null);

            _mediator.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default))
                .ReturnsAsync(new Product
                {
                    ProductId = 1,
                    Name = "First",
                    Description = "Desc",
                    Price = 1,
                    CategoryId = 1,
                    SupplierId = 1
                });

            //Act
            var response = _controller.UpdateProduct(updateProduct).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Product>();
        }

        [Fact]
        public void UpdateProduct_WhenCalled_ReturnNotFound()
        {
            //Arrange
            UpdateProductRequest updateProduct = new(1, "First", "Desc", 1, 1, 1, null);

            _mediator.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default))
                .ReturnsAsync((Product)null!);

            //Act
            var response = _controller.UpdateProduct(updateProduct).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region DeleteProduct

        [Fact]
        public void DeleteProduct_WhenCalled_ReturnOk()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), default))
                .ReturnsAsync(new Product
                {
                    ProductId = 1,
                    Name = "First",
                    Description = "Desc",
                    Price = 1,
                    CategoryId = 1,
                    SupplierId = 1
                });

            //Act
            var response = _controller.DeleteProduct(1).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Product>();
        }

        [Fact]
        public void DeleteProduct_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), default))
                .ReturnsAsync((Product)null!);

            //Act
            var response = _controller.DeleteProduct(1).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion
    }
}
