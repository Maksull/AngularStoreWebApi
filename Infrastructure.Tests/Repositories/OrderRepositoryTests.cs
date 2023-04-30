using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public sealed class OrderRepositoryTests
    {
        [Fact]
        public void Orders_WhenCalled_ReturnOrders()
        {
            Order[] orders =
            {
                new()
                {
                    OrderId = 1, Name = "First", Email = "a@a.co", Address = "address", City = "CityFirst", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                new()
                {
                    OrderId = 2, Name = "Second", Email = "a@a.co", Address = "address", City = "CitySecond", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                new()
                {
                    OrderId = 3, Name = "First", Email = "a@a.co", Address = "address", City = "CityFirst", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "Orders")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                OrderRepository orderRepository = new(context);
                var result = (orderRepository.Orders.AsNoTracking()).ToArray();

                result.Should().BeOfType<Order[]>();
                result.Should().BeEquivalentTo(orders, opts => opts.Excluding(o => o.Lines));
            }
        }

        [Fact]
        public async void CreateOrderAsync_WhenCalled_AddOrder()
        {
            Order[] orders =
            {
                new()
                {
                    OrderId = 1, Name = "First", Email = "a@a.co", Address = "address", City = "CityFirst", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                new()
                {
                    OrderId = 2, Name = "Second", Email = "a@a.co", Address = "address", City = "CitySecond", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                new()
                {
                    OrderId = 3, Name = "First", Email = "a@a.co", Address = "address", City = "CityFirst", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
            };
            Order order = new()
            {
                OrderId = 0,
                Name = "First",
                Email = "a@a.co",
                Address = "address",
                City = "CityFirst",
                Country = "Country",
                Zip = "Zip",
                Lines = new List<CartLine>()
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "CreateOrder")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                OrderRepository orderRepository = new(context);
                var oldResult = (orderRepository.Orders.AsNoTracking()).ToArray();
                await orderRepository.CreateOrderAsync(order);
                var newResult = (orderRepository.Orders.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Order[]>();
                oldResult.Should().BeEquivalentTo(orders, opts => opts.Excluding(o => o.Lines));
                newResult.Should().BeOfType<Order[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(orders.Length + 1);
            }
        }

        [Fact]
        public async void UpdateOrderAsync_WhenCalled_UpdateOrder()
        {
            Order order = new()
            {
                OrderId = 2,
                Name = "First",
                Email = "a@a.co",
                Address = "address",
                City = "CityFirst",
                Country = "Country",
                Zip = "Zip",
                Lines = new List<CartLine>()
            };
            Order[] orders =
            {
                new()
                {
                    OrderId = 1, Name = "First", Email = "a@a.co", Address = "address", City = "CityFirst", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                order,
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateOrder")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                OrderRepository orderRepository = new(context);
                order.Name = "UpdatedName";
                order.Address = "UpdatedAddress";

                await orderRepository.UpdateOrderAsync(order);
                var result = (orderRepository.Orders.AsNoTracking()).ToArray();

                result.Should().BeOfType<Order[]>();
                result.Should().HaveCount(orders.Length);
                result[1].Should().BeEquivalentTo(order, opts => opts.Excluding(o => o.Lines));
            }
        }

        [Fact]
        public async void DeleteOrderAsync_WhenCalled_DeleteOrder()
        {
            Order order = new()
            {
                OrderId = 3,
                Name = "First",
                Email = "a@a.co",
                Address = "address",
                City = "CityFirst",
                Country = "Country",
                Zip = "Zip",
                Lines = new List<CartLine>()
            };
            Order[] orders =
            {
                new()
                {
                    OrderId = 1, Name = "First", Email = "a@a.co", Address = "address", City = "CityFirst", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                new()
                {
                    OrderId = 2, Name = "Second", Email = "a@a.co", Address = "address", City = "CitySecond", Country = "Country", Zip = "Zip", Lines = new List<CartLine>()
                },
                order,
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "DeleteOrder")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                OrderRepository orderRepository = new(context);
                var oldResult = (orderRepository.Orders.AsNoTracking()).ToArray();
                await orderRepository.DeleteOrderAsync(order);
                var newResult = (orderRepository.Orders.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Order[]>();
                oldResult.Should().BeEquivalentTo(orders, opts => opts.Excluding(o => o.Lines));
                newResult.Should().BeOfType<Order[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(orders.Length - 1);
            }
        }
    }
}
