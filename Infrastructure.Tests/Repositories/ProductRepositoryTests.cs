using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public sealed class ProductRepositoryTests
    {
        [Fact]
        public void Products_WhenCalled_ReturnProducts()
        {
            Product[] products =
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
                new()
                {
                    ProductId = 2, Name = "First", Description = "Desc", Price = 1, CategoryId = 1, SupplierId = 1
                },
                new()
                {
                    ProductId = 3, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "Products")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                ProductRepository productRepository = new(context);
                var result = (productRepository.Products.AsNoTracking()).ToArray();

                result.Should().BeOfType<Product[]>();
                result.Should().BeEquivalentTo(products);
            }
        }

        [Fact]
        public async void CreateProductAsync_WhenCalled_AddProduct()
        {
            Product[] products =
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
                new()
                {
                    ProductId = 2, Name = "First", Description = "Desc", Price = 1, CategoryId = 1, SupplierId = 1
                },
                new()
                {
                    ProductId = 3, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1, SupplierId = 1
                },
            };
            Product product = new()
            {
                ProductId = 0,
                Name = "First",
                Description = "Desc",
                Price = 1,
                CategoryId = 1,
                SupplierId = 1
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "CreateProduct")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                ProductRepository productRepository = new(context);
                var oldResult = (productRepository.Products.AsNoTracking()).ToArray();
                await productRepository.CreateProductAsync(product);
                var newResult = (productRepository.Products.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Product[]>();
                oldResult.Should().BeEquivalentTo(products);
                newResult.Should().BeOfType<Product[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(products.Length + 1);
            }
        }

        [Fact]
        public async void UpdateProductAsync_WhenCalled_UpdateProduct()
        {
            Product product = new()
            {
                ProductId = 2,
                Name = "First",
                Description = "Desc",
                Price = 1,
                CategoryId = 1,
                SupplierId = 1
            };
            Product[] products =
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1,  SupplierId = 1
                },
                product,
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateProduct")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                ProductRepository productRepository = new(context);
                product.Name = "UpdatedName";
                product.Description = "NewDescription";

                await productRepository.UpdateProductAsync(product);
                var result = (productRepository.Products.AsNoTracking()).ToArray();

                result.Should().BeOfType<Product[]>();
                result.Should().HaveCount(products.Length);
                result[1].Should().BeEquivalentTo(product);
            }
        }

        [Fact]
        public async void DeleteProductAsync_WhenCalled_DeleteProduct()
        {
            Product product = new()
            {
                ProductId = 3,
                Name = "First",
                Description = "Desc",
                Price = 1,
                CategoryId = 1,
                SupplierId = 1

            };
            Product[] products =
            {
                new()
                {
                    ProductId = 1, Name = "First", Description = "Desc", Price = 1 , CategoryId = 1,  SupplierId = 1
                },
                new()
                {
                    ProductId = 2, Name = "First", Description = "Desc", Price = 1, CategoryId = 1, SupplierId = 1
                },
                product,
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "DeleteProduct")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                ProductRepository productRepository = new(context);
                var oldResult = (productRepository.Products.AsNoTracking()).ToArray();
                await productRepository.DeleteProductAsync(product);
                var newResult = (productRepository.Products.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Product[]>();
                oldResult.Should().BeEquivalentTo(products);
                newResult.Should().BeOfType<Product[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(products.Length - 1);
            }
        }
    }
}
