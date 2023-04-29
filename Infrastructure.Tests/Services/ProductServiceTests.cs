using Core.Contracts.Controllers.Products;
using Core.Entities;
using Infrastructure.Mapster;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;
using System.Text;

namespace Infrastructure.Tests.Services
{
    public sealed class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<ICacheService> _cacheService;
        private readonly Mock<IS3Service> _s3Service;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _unitOfWork = new();
            _mapper = GetMapper();
            _s3Service = new();
            _cacheService = new();
            _productService = new(_unitOfWork.Object, _mapper, _s3Service.Object, _cacheService.Object); ;
        }


        #region GetProducts

        [Fact]
        public void GetProducts_WhenCalled_ReturnProducts()
        {
            //Arrange
            Product[] products = new Product[]
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , Category = new(), CategoryId = 1, Supplier = new(),  SupplierId = 1
                },
                new()
                {
                    ProductId = 2, Name = "First", Description = "Desc", Price = 1, Category = new(), CategoryId = 1, Supplier = new(), SupplierId = 1
                },
                new()
                {
                    ProductId = 3, Name = "First", Description = "Desc", Price = 1 , Category = new(), CategoryId = 1, Supplier = new(), SupplierId = 1
                },
            };

            _unitOfWork.Setup(u => u.Product.Products).Returns(products.AsQueryable());

            //Act
            var result = _productService.GetProducts().ToArray();


            //Arrange
            result.Should().BeOfType<Product[]>();
            result.Should().BeEquivalentTo(products);
        }

        #endregion


        #region GetProduct

        [Fact]
        public void GetProduct_WhenCache_ReturnProduct()
        {
            //Arrange
            Product product = new()
            {
                ProductId = 2,
                Name = "First",
                Description = "Desc",
                Price = 1,
                CategoryId = 1,
                SupplierId = 1,
            };
            _cacheService.Setup(u => u.GetAsync<Product>(It.IsAny<string>())).ReturnsAsync(product);

            //Act
            var result = _productService.GetProduct(1).Result;

            //Assert
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void GetProduct_WhenNoCache_ReturnProduct()
        {
            //Arrange
            Product[] products = new Product[]
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , Category = new(), CategoryId = 1, Supplier = new(), SupplierId = 1
                },
                new()
                {
                    ProductId = 2, Name = "First", Description = "Desc", Price = 1 , Category = new(), CategoryId = 1, Supplier = new(), SupplierId = 1
                },
                new()
                {
                    ProductId = 3, Name = "First", Description = "Desc", Price = 1 , Category = new(), CategoryId = 1, Supplier = new(), SupplierId = 1
                },
            };
            var mock = products.AsQueryable().BuildMock();

            _cacheService.Setup(u => u.GetAsync<Product>(It.IsAny<string>())).ReturnsAsync((Product)null!);
            _unitOfWork.Setup(u => u.Product.Products).Returns(mock);

            //Act
            var result = _productService.GetProduct(1).Result;

            //Assert
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(products[0]);
        }

        #endregion


        #region CreateProduct

        [Fact]
        public void CreateProduct_WhenCalled_ReturnProduct()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            CreateProductRequest createProduct = new("First", "Desc", 1, 1, 1, file);
            Product product = new()
            {
                Name = "First",
                Description = "Desc",
                Price = 1,
                CategoryId = 1,
                SupplierId = 1,
                Images = $"First/{file.FileName}"
            };
            _unitOfWork.Setup(u => u.Product.CreateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            //Act
            var result = _productService.CreateProduct(createProduct).Result;

            //Assert
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(product);
        }

        #endregion


        #region UpdateProduct

        [Fact]
        public void UpdateProduct_WhenCalled_ReturnProduct()
        {
            //Arrange
            UpdateProductRequest updateProduct = new(1, "First", "Desc", 1, 1, 1, null);

            Product[] products = { new() {
                    ProductId = 1,
                    Name = "First",
                    Description = "Desc",
                    Price = 1,
                    CategoryId = 1,
                    SupplierId = 1
                }};

            var mock = products.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Product.Products).Returns(mock);

            //Act
            var result = _productService.UpdateProduct(updateProduct).Result;

            //Assert
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(products[0]);
        }

        [Fact]
        public void UpdateProduct_WhenCalled_ReturnNull()
        {
            //Arrange
            UpdateProductRequest updateProduct = new(1, "First", "Desc", 1, 1, 1, null);
            Product[] products = Array.Empty<Product>();
            var mock = products.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Product.Products).Returns(mock);

            //Act
            var result = _productService.UpdateProduct(updateProduct).Result;

            //Assert
            result.Should().BeNull();
        }

        #endregion


        #region DeleteProduct

        [Fact]
        public void DeleteProduct_WhenCalled_ReturnProduct()
        {
            //Arrange
            Product[] products = new Product[]
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , Category = new(), CategoryId = 1, Supplier = new(),  SupplierId = 1
                },
                new()
                {
                    ProductId = 2, Name = "First", Description = "Desc", Price = 1, Category = new(), CategoryId = 1, Supplier = new(), SupplierId = 1
                },
                new()
                {
                    ProductId = 3, Name = "First", Description = "Desc", Price = 1 , Category = new(), CategoryId = 1, Supplier = new(), SupplierId = 1
                },
            };

            var mock = products.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Product.Products).Returns(mock);

            //Act
            var result = _productService.DeleteProduct(1).Result;

            //Assert
            result.Should().BeOfType<Product>();
            result.Should().BeEquivalentTo(products[0]);
        }

        [Fact]
        public void DeleteProduct_WhenCalled_ReturnNull()
        {
            //Arrange
            Product[] Products = Array.Empty<Product>();
            var mock = Products.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Product.Products).Returns(mock);

            //Act
            var result = _productService.DeleteProduct(1).Result;

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
