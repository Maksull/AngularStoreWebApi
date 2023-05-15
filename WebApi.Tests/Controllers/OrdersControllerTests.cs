using Core.Contracts.Controllers.Orders;
using Core.Entities;
using Core.Mediator.Commands.Orders;
using Core.Mediator.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Controllers
{
    public sealed class OrdersControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mediator = new();
            _controller = new(_mediator.Object);
        }

        #region GetOrders

        [Fact]
        public void GetOrders_WhenCalled_ReturnOk()
        {
            //Arrange
            var orders = new List<Order>()
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
            _mediator.Setup(m => m.Send(It.IsAny<GetOrdersQuery>(), default))
                .ReturnsAsync(orders);

            //Act
            var response = (_controller.GetOrders().Result as OkObjectResult)!;
            var result = response.Value as List<Order>;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<List<Order>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(orders);
        }

        [Fact]
        public void GetOrders_WhenCalled_ReturnNotFound()
        {
            //Arrange
            var orders = new List<Order>();
            _mediator.Setup(m => m.Send(It.IsAny<GetOrdersQuery>(), default))
                .ReturnsAsync(orders);


            //Act
            var response = _controller.GetOrders().Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region GetOrdersByUserId

        [Fact]
        public void GetOrdersByUserId_WhenCalled_ReturnOk()
        {
            //Arrange
            var orders = new List<Order>()
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
            _mediator.Setup(m => m.Send(It.IsAny<GetOrdersByUserIdQuery>(), default))
                .ReturnsAsync(orders);

            //Act
            var response = (_controller.GetOrdersByUserId().Result as OkObjectResult)!;
            var result = response.Value as List<Order>;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<List<Order>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(orders);
        }

        [Fact]
        public void GetOrdersByUserId_WhenCalled_ReturnNotFound()
        {
            //Arrange
            var orders = new List<Order>();
            _mediator.Setup(m => m.Send(It.IsAny<GetOrdersByUserIdQuery>(), default))
                .ReturnsAsync(orders);


            //Act
            var response = _controller.GetOrdersByUserId().Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region GetOrder

        [Fact]
        public void GetOrder_WhenCalled_ReturnOk()
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
                Lines = new List<CartLine>()

            };
            _mediator.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), default))
                .ReturnsAsync(order);

            //Act
            var response = (_controller.GetOrder(1).Result as OkObjectResult)!;
            var result = response.Value as Order;

            //Assert
            response.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<Order>();
            result.Should().BeEquivalentTo(order);
        }

        [Fact]
        public void GetOrder_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), default))
                .ReturnsAsync((Order)null!);

            //Act
            var response = _controller.GetOrder(1).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region CreateOrder

        [Fact]
        public void CreateOrder_WhenCalled_ReturnOk()
        {
            //Arrange
            CreateOrderRequest createOrder = new("First", "a@a.co", "address", "CityFirst", "Country", "Zip", new List<CreateCartLineRequest>());

            _mediator.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), default))
                .ReturnsAsync(new Order
                {
                    OrderId = 1,
                    Name = "First",
                    Email = "a@a.co",
                    Address = "address",
                    City = "CityFirst",
                    Country = "Country",
                    Zip = "Zip",
                    Lines = new List<CartLine>()
                });

            //Act
            var response = _controller.CreateOrder(createOrder).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Order>();
        }

        #endregion

        #region UpdateOrder

        [Fact]
        public void UpdateOrder_WhenCalled_ReturnOk()
        {
            //Arrange
            UpdateOrderRequest updateOrder = new(1, "First", "a@a.co", "address", "CityFirst", "Country", "Zip", false, new List<UpdateCartLineRequest>());

            _mediator.Setup(m => m.Send(It.IsAny<UpdateOrderCommand>(), default))
                .ReturnsAsync(new Order
                {
                    OrderId = 1,
                    Name = "First",
                    Email = "a@a.co",
                    Address = "address",
                    City = "CityFirst",
                    Country = "Country",
                    Zip = "Zip",
                    Lines = new List<CartLine>()
                });

            //Act
            var response = _controller.UpdateOrder(updateOrder).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Order>();
        }

        [Fact]
        public void UpdateOrder_WhenCalled_ReturnNotFound()
        {
            //Arrange
            UpdateOrderRequest updateOrder = new(1, "First", "a@a.co", "address", "CityFirst", "Country", "Zip", false, new List<UpdateCartLineRequest>());

            _mediator.Setup(m => m.Send(It.IsAny<UpdateOrderCommand>(), default))
                .ReturnsAsync((Order)null!);

            //Act
            var response = _controller.UpdateOrder(updateOrder).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region DeleteOrder

        [Fact]
        public void DeleteOrder_WhenCalled_ReturnOk()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteOrderCommand>(), default))
                .ReturnsAsync(new Order
                {
                    OrderId = 1,
                    Name = "First",
                    Email = "a@a.co",
                    Address = "address",
                    City = "CityFirst",
                    Country = "Country",
                    Zip = "Zip",
                    Lines = new List<CartLine>()
                });

            //Act
            var response = _controller.DeleteOrder(1).Result;
            var result = (response as OkObjectResult)!;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().BeOfType<Order>();
        }

        [Fact]
        public void DeleteOrder_WhenCalled_ReturnNotFound()
        {
            //Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteOrderCommand>(), default))
                .ReturnsAsync((Order)null!);

            //Act
            var response = _controller.DeleteOrder(1).Result;
            var result = response as NotFoundResult;

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion
    }
}
