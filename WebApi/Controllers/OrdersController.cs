using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public sealed class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;

        public OrdersController(IOrderRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOrders()
        {
            if (_repository.Orders != null)
            {
                return Ok(_repository.Orders.Include(o => o.Lines)!.ThenInclude(l => l.Product));
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(long id)
        {
            if(_repository.Orders != null)
            {
                Order? o = await _repository.Orders.Include(o => o.Lines)!.ThenInclude(l => l.Product).FirstOrDefaultAsync(o => o.OrderId == id);

                if(o != null)
                {
                    return Ok(o);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            await _repository.CreateOrderAsync(order);
            return Ok(order);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            if(await _repository.Orders.ContainsAsync(order))
            {
                await _repository.UpdateOrderAsync(order);
                return Ok(order);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            if(_repository.Orders != null)
            {
                Order? o = await _repository.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

                if(o != null)
                {
                    await _repository.DeleteOrderAsync(o);
                    return Ok(o);
                }
            }
            return NotFound();
        }
    }
}
