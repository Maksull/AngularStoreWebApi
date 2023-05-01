using Core.Contracts.Controllers.Orders;
using Core.Entities;
using Infrastructure.Mapster;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using Mapster;
using MapsterMapper;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.Tests.Services
{
    public sealed class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<ICacheService> _cacheService;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _unitOfWork = new();
            _mapper = GetMapper();
            _emailService = new();
            _cacheService = new();
            _orderService = new(_unitOfWork.Object, _mapper, _emailService.Object, _cacheService.Object); ;
        }


        #region GetOrders

        [Fact]
        public void GetOrders_WhenCalled_ReturnOrders()
        {
            //Arrange
            Order[] orders = new Order[]
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

            _unitOfWork.Setup(u => u.Order.Orders).Returns(orders.AsQueryable());

            //Act
            var result = _orderService.GetOrders().ToArray();


            //Arrange
            result.Should().BeOfType<Order[]>();
            result.Should().BeEquivalentTo(orders);
        }

        #endregion


        #region GetOrder

        [Fact]
        public void GetOrder_WhenCache_ReturnOrder()
        {
            //Arrange
            Order order = new()
            {
                OrderId = 1,
                Name = "First",
                Email = "a@a.co",
                Address = "address",
                City = "CityFirst",
                Country = "Country",
                Zip = "Zip",
                Lines = new List<CartLine>(),
            };
            _cacheService.Setup(u => u.GetAsync<Order>(It.IsAny<string>())).ReturnsAsync(order);

            //Act
            var result = _orderService.GetOrder(1).Result;

            //Assert
            result.Should().BeOfType<Order>();
            result.Should().BeEquivalentTo(order);
        }

        [Fact]
        public void GetOrder_WhenNoCache_ReturnOrder()
        {
            //Arrange
            Order[] orders = new Order[]
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
            var mock = orders.AsQueryable().BuildMock();

            _cacheService.Setup(u => u.GetAsync<Order>(It.IsAny<string>())).ReturnsAsync((Order)null!);
            _unitOfWork.Setup(u => u.Order.Orders).Returns(mock);

            //Act
            var result = _orderService.GetOrder(1).Result;

            //Assert
            result.Should().BeOfType<Order>();
            result.Should().BeEquivalentTo(orders[0]);
        }

        #endregion


        #region CreateOrder

        [Fact]
        public void CreateOrder_WhenCalled_ReturnOrder()
        {
            //Arrange
            CreateOrderRequest createOrder = new("First", "a@a.co", "address", "CityFirst", "Country", "Zip", new List<CreateCartLineRequest>());
            Order order = new()
            {
                OrderId = 0,
                Name = "First",
                Email = "a@a.co",
                Address = "address",
                City = "CityFirst",
                Country = "Country",
                Zip = "Zip",
                Lines = new List<CartLine>(),
            };
            _unitOfWork.Setup(u => u.Order.CreateOrderAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            //Act
            var result = _orderService.CreateOrder(createOrder).Result;

            //Assert
            result.Should().BeOfType<Order>();
            result.Should().BeEquivalentTo(order);
        }

        #endregion


        #region UpdateOrder

        [Fact]
        public void UpdateOrder_WhenCalled_ReturnOrder()
        {
            //Arrange
            UpdateOrderRequest updateOrder = new(1, "First", "a@a.co", "address", "CityFirst", "Country", "Zip", false, new List<UpdateCartLineRequest>());

            Order[] orders = new Order[]
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

            var mock = orders.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Order.Orders).Returns(mock);

            //Act
            var result = _orderService.UpdateOrder(updateOrder).Result;

            //Assert
            result.Should().BeOfType<Order>();
        }

        [Fact]
        public void UpdateOrder_WhenCalled_ReturnNull()
        {
            //Arrange
            UpdateOrderRequest updateOrder = new(1, "First", "a@a.co", "address", "CityFirst", "Country", "Zip", false, new List<UpdateCartLineRequest>());
            Order[] Orders = Array.Empty<Order>();
            var mock = Orders.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Order.Orders).Returns(mock);

            //Act
            var result = _orderService.UpdateOrder(updateOrder).Result;

            //Assert
            result.Should().BeNull();
        }

        #endregion


        #region DeleteOrder

        [Fact]
        public void DeleteOrder_WhenCalled_ReturnOrder()
        {
            //Arrange
            Order[] orders = new Order[]
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

            var mock = orders.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Order.Orders).Returns(mock);

            //Act
            var result = _orderService.DeleteOrder(1).Result;

            //Assert
            result.Should().BeOfType<Order>();
        }

        [Fact]
        public void DeleteOrder_WhenCalled_ReturnNull()
        {
            //Arrange
            Order[] Orders = Array.Empty<Order>();
            var mock = Orders.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Order.Orders).Returns(mock);

            //Act
            var result = _orderService.DeleteOrder(1).Result;

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
