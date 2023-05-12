using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Core.Mediator.Queries.Suppliers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public sealed class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuppliersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Supplier>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _mediator.Send(new GetSuppliersQuery());

            if (suppliers.Any())
            {
                return Ok(suppliers);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Supplier))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetSupplier(long id)
        {
            var supplier = await _mediator.Send(new GetSupplierByIdQuery(id));

            if (supplier != null)
            {
                return Ok(supplier);
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Supplier))]
        public async Task<IActionResult> CreateSupplier(CreateSupplierRequest createSupplier)
        {
            var s = await _mediator.Send(new CreateSupplierCommand(createSupplier));

            return Ok(s);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Supplier))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateSupplier(UpdateSupplierRequest updateSupplier)
        {
            var s = await _mediator.Send(new UpdateSupplierCommand(updateSupplier));

            if (s != null)
            {
                return Ok(s);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Supplier))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteSupplier(long id)
        {
            var supplier = await _mediator.Send(new DeleteSupplierCommand(id));

            if (supplier != null)
            {
                return Ok(supplier);
            }

            return NotFound();
        }
    }
}
