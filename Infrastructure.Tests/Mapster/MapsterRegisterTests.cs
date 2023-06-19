using Core.Contracts.Controllers.Auth;
using Core.Contracts.Controllers.Categories;
using Core.Contracts.Controllers.Orders;
using Core.Contracts.Controllers.Products;
using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using Infrastructure.Mapster;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Infrastructure.Tests.Mapster
{
    public sealed class MapsterRegisterTests
    {
        private readonly Mapper _mapper;

        public MapsterRegisterTests()
        {
            TypeAdapterConfig config = new();
            config.Apply(new MapsterRegister());

            _mapper = new Mapper(config);
        }

        [Fact]
        public void Map_Products()
        {
            //Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            Product product = new()
            {
                ProductId = 1,
                Name = "First",
                Description = "Desc",
                Price = 1,
                CategoryId = 1,
                SupplierId = 1,
                Images = $"First/{file.FileName}"
            };

            CreateProductRequest createProductRequest = new(product.Name, product.Description, product.Price, product.CategoryId, product.SupplierId, file);
            UpdateProductRequest updateProductRequest = new(1, product.Name, product.Description, product.Price, product.CategoryId, product.SupplierId, file);

            //Act
            var mappedProduct = _mapper.Map<Product>(createProductRequest);
            var mappedUpdateProduct = _mapper.Map<Product>(updateProductRequest);

            //Assert
            mappedProduct.ProductId.Should().Be(0);
            mappedProduct.Name.Should().Be(product.Name);
            mappedProduct.Description.Should().Be(product.Description);
            mappedProduct.Price.Should().Be(product.Price);
            mappedProduct.CategoryId.Should().Be(product.CategoryId);
            mappedProduct.SupplierId.Should().Be(product.SupplierId);
            mappedProduct.Images.Should().Be(product.Images);

            mappedUpdateProduct.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void Map_Categories()
        {
            //Arrange
            Category category = new()
            {
                CategoryId = 1,
                Name = "TestName",
            };

            CreateCategoryRequest createCategoryRequest = new(category.Name);
            UpdateCategoryRequest updateCategoryRequest = new(category.CategoryId, category.Name);

            //Act
            var mappedCategory = _mapper.Map<Category>(createCategoryRequest);
            var mappedUpdateCategory = _mapper.Map<Category>(updateCategoryRequest);

            //Assert
            mappedCategory.CategoryId.Should().Be(0);
            mappedCategory.Name.Should().Be(category.Name);
            mappedUpdateCategory.Should().BeEquivalentTo(category);
        }

        [Fact]
        public void Map_Suppliers()
        {
            //Arrange
            Supplier supplier = new()
            {
                SupplierId = 1,
                Name = "TestName",
                City = "TestCity",
            };

            CreateSupplierRequest createSupplierRequest = new(supplier.Name, supplier.City);
            UpdateSupplierRequest updateSupplierRequest = new(supplier.SupplierId, supplier.Name, supplier.City);

            //Act
            var mappedSupplier = _mapper.Map<Supplier>(createSupplierRequest);
            var mappedUpdateSupplier = _mapper.Map<Supplier>(updateSupplierRequest);

            //Assert
            mappedSupplier.SupplierId.Should().Be(0);
            mappedSupplier.Name.Should().Be(supplier.Name);
            mappedSupplier.City.Should().Be(supplier.City);
            mappedUpdateSupplier.Should().BeEquivalentTo(supplier);
        }

        [Fact]
        public void Map_Orders()
        {
            //Arrange
            Order order = new()
            {
                OrderId = 1,
                Name = "TestName",
                Email = "a@a.co",
                Address = "TestAddress",
                City = "TestCity",
                Country = "TestCountry",
                Zip = "Zip",
                IsShipped = false,
                Lines = new List<CartLine>()
            };

            CreateOrderRequest createOrderRequest = new(order.Name, order.Email, order.Address, order.City, order.Country, order.Zip, new List<CreateCartLineRequest>());
            UpdateOrderRequest updateOrderRequest = new(order.OrderId, order.Name, order.Email, order.Address, order.City, order.Country, order.Zip, order.IsShipped, new List<UpdateCartLineRequest>());

            //Act
            var mappedOrder = _mapper.Map<Order>(createOrderRequest);
            var mappedUpdateOrder = _mapper.Map<Order>(updateOrderRequest);

            //Assert
            mappedOrder.OrderId.Should().Be(0);
            mappedOrder.Name.Should().Be(order.Name);
            mappedOrder.Email.Should().Be(order.Email);
            mappedOrder.Address.Should().Be(order.Address);
            mappedOrder.City.Should().Be(order.City);
            mappedOrder.Country.Should().Be(order.Country);
            mappedOrder.Zip.Should().Be(order.Zip);
            mappedOrder.Lines.Should().BeEquivalentTo(order.Lines);

            mappedUpdateOrder.Should().BeEquivalentTo(order);
        }

        [Fact]
        public void Map_CartLines()
        {
            //Arrange
            CartLine cartLine = new()
            {
                CartLineId = 1,
                ProductId = 1,
                Quantity = 1,
                OrderId = 1,
            };

            CreateCartLineRequest createCartLineRequest = new(cartLine.ProductId, cartLine.Quantity);
            UpdateCartLineRequest updateCartLineRequest = new(cartLine.CartLineId, cartLine.ProductId, cartLine.Quantity, cartLine.OrderId);

            //Act
            var mappedCartLine = _mapper.Map<CartLine>(createCartLineRequest);
            var mappedUpdateCartLine = _mapper.Map<CartLine>(updateCartLineRequest);

            //Assert
            mappedCartLine.ProductId.Should().Be(cartLine.ProductId);
            mappedCartLine.Quantity.Should().Be(cartLine.Quantity);
            mappedUpdateCartLine.Should().BeEquivalentTo(cartLine);
        }

        [Fact]
        public void Map_Users()
        {
            //Arrange
            User user = new()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                UserName = "UserName",
                Email = "email",
                PhoneNumber = "+380554441618",
                RefreshToken = "TestRefreshToken",
                RefreshTokenExpired = DateTime.Now,
            };

            RegisterRequest registerRequest = new(user.FirstName, user.LastName, user.UserName, user.Email, user.PhoneNumber, "Password", "Password");

            //Act
            var mappedRegisterRequest = _mapper.Map<User>(registerRequest);

            //Assert
            mappedRegisterRequest.FirstName.Should().Be(user.FirstName);
            mappedRegisterRequest.LastName.Should().Be(user.LastName);
            mappedRegisterRequest.UserName.Should().Be(user.UserName);
            mappedRegisterRequest.Email.Should().Be(user.Email);
            mappedRegisterRequest.PhoneNumber.Should().Be(user.PhoneNumber);
        }

        [Fact]
        public void Map_RefreshToken()
        {
            //Arrange
            RefreshToken refreshToken = new()
            {
                Token = "Token",
                Expired = DateTime.Now,
            };

            RefreshTokenRequest refreshTokenRequest = new(refreshToken.Token, refreshToken.Expired);

            //Act
            var mappedRefreshToken = _mapper.Map<RefreshToken>(refreshTokenRequest);

            //Assert
            mappedRefreshToken.Should().BeEquivalentTo(refreshToken);
        }
    }
}
