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

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _mediator.Send(new GetOrdersQuery());

            if (orders.Any())
            {
                return Ok(orders);
            }

            return NotFound();
        }

        [HttpGet("userId")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            var orders = await _mediator.Send(new GetOrdersByUserIdQuery(User));

            if (orders.Any())
            {
                return Ok(orders);
            }

            return NotFound();
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetOrder(long id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));

            if (order != null)
            {
                return Ok(order);
            }

            return NotFound();
        }


        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest order)
        {
            var o = await _mediator.Send(new CreateOrderCommand(order, User));

            return Ok(o);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateOrder(UpdateOrderRequest order)
        {
            var o = await _mediator.Send(new UpdateOrderCommand(order));

            if (o != null)
            {
                return Ok(o);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var order = await _mediator.Send(new DeleteOrderCommand(id));

            if (order != null)
            {
                return Ok(order);
            }

            return NotFound();
        }
    }
}
