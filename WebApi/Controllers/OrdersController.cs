using Core.Contracts.Controllers.Orders;
using Core.Entities;
using Core.Mediator.Commands.Orders;
using Core.Mediator.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public sealed class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public OrdersController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of orders.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/orders
        ///     
        /// </remarks>
        /// <returns>Returns the list of orders</returns>
        /// <response code="200">Returns the list of orders</response>
        /// <response code="404">If the orders do not exist</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _mediator.Send(new GetOrdersQuery());

            if (orders.Any())
            {
                _logger.Information("Orders found. Count: {OrdersCount}.", orders.Count());

                return Ok(orders);
            }
            _logger.Information("No Orders found.");

            return NotFound();
        }

        /// <summary>
        /// Gets an orders by user's id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/orders/
        ///     
        /// </remarks>
        /// <returns>Returns the list of orders with same user's id</returns>
        /// <response code="200">Returns the list of orders with same user's id</response>
        /// <response code="404">If the orders does not exist</response>
        [HttpGet("userId")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            var orders = await _mediator.Send(new GetOrdersByUserIdQuery(User));

            if (orders.Any())
            {
                _logger.Information("Orders found. Count: {OrdersCount}.", orders.Count());

                return Ok(orders);
            }
            _logger.Information("No Orders found.");

            return NotFound();
        }

        /// <summary>
        /// Gets an order by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/orders/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the order. This ID is used to retrieve a specific order from the database.</param>
        /// <returns>A order</returns>
        /// <response code="200">Returns the order</response>
        /// <response code="404">If the order does not exist</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetOrder([FromRoute] long id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));

            if (order != null)
            {
                _logger.Information("Order found. OrderId: {OrderId}.", id);

                return Ok(order);
            }
            _logger.Information("Order not found. OrderId: {OrderId}.", id);

            return NotFound();
        }

        /// <summary>
        /// Creates an order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/orders
        ///     {        
        ///       "name": "Username",
        ///       "email": "your_email@sol.com",
        ///       "address": "Address",
        ///       "city": "City",
        ///       "country": "Country",
        ///       "zip": "23327",
        ///       "lines": [
        ///         {
        ///           "productId": 1,
        ///           "quantity": 2
        ///         },
        ///         {
        ///           "productId": 3,
        ///           "quantity": 1
        ///         },
        ///       ]
        ///     }
        /// </remarks>
        /// <param name="createOrder">The order object containing the details of the order to be created.</param>
        /// <returns>A newly created order</returns>
        /// <response code="200">Returns the newly created order</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest createOrder)
        {
            var o = await _mediator.Send(new CreateOrderCommand(createOrder, User));

            _logger.Information("Order created. OrderId: {OrderId}.", o.OrderId);

            return Ok(o);
        }

        /// <summary>
        /// Updates an order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/orders
        ///     {    
        ///       "orderId": 1,
        ///       "name": "Username",
        ///       "email": "your_email@sol.com",
        ///       "address": "Address",
        ///       "city": "City",
        ///       "country": "Country",
        ///       "zip": "23327",
        ///       "lines": [
        ///         {
        ///           "productId": 1,
        ///           "quantity": 2
        ///         },
        ///         {
        ///           "productId": 3,
        ///           "quantity": 1
        ///         },
        ///       ]
        ///     }
        /// </remarks>
        /// <param name="updateOrder">The order object containing the details of the order to be updated.</param>
        /// <returns>An updated order</returns>
        /// <response code="200">Returns the updated order</response>
        /// <response code="404">If the order does not exist</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest updateOrder)
        {
            var o = await _mediator.Send(new UpdateOrderCommand(updateOrder));

            if (o != null)
            {
                _logger.Information("Order updated. OrderId: {OrderId}.", o.OrderId);

                return Ok(o);
            }
            _logger.Information("Order not found. OrderId: {OrderId}.", updateOrder.OrderId);

            return NotFound();
        }

        /// <summary>
        /// Deletes an order by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/orders/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the order. This ID is used to delete a specific order from the database.</param>
        /// <returns>An deleted order</returns>
        /// <response code="200">Returns the deleted order</response>
        /// <response code="404">If the order does not exist</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var order = await _mediator.Send(new DeleteOrderCommand(id));

            if (order != null)
            {
                _logger.Information("Order deleted. OrderId: {OrderId}.", id);

                return Ok(order);
            }
            _logger.Information("Order not found. OrderId: {OrderId}.", id);

            return NotFound();
        }
    }
}
