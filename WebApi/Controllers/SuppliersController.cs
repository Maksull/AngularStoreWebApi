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

        /// <summary>
        /// Gets a list of suppliers.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/suppliers
        ///     
        /// </remarks>
        /// <returns>Returns the list of suppliers</returns>
        /// <response code="200">Returns the list of suppliers</response>
        /// <response code="404">If the suppliers do not exist</response>
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

        /// <summary>
        /// Gets a supplier by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/suppliers/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the supplier. This ID is used to retrieve a specific supplier from the database.</param>
        /// <returns>A supplier</returns>
        /// <response code="200">Returns the supplier</response>
        /// <response code="404">If the supplier does not exist</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Supplier))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetSupplier([FromRoute] long id)
        {
            var supplier = await _mediator.Send(new GetSupplierByIdQuery(id));

            if (supplier != null)
            {
                return Ok(supplier);
            }

            return NotFound();
        }

        /// <summary>
        /// Creates a supplier.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/suppliers
        ///     {        
        ///       "name": "GoBikes",
        ///       "city": "Ohio",
        ///     }
        /// </remarks>
        /// <param name="createSupplier">The supplier object containing the details of the supplier to be created.</param>
        /// <returns>A newly created supplier</returns>
        /// <response code="200">Returns the newly created supplier</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Supplier))]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequest createSupplier)
        {
            var s = await _mediator.Send(new CreateSupplierCommand(createSupplier));

            return Ok(s);
        }

        /// <summary>
        /// Updates a supplier.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/suppliers
        ///     {        
        ///       "categoryId": 1,
        ///       "name": "GoBikes",
        ///       "city": "Ohio"
        ///     }
        /// </remarks>
        /// <param name="updateSupplier">The supplier object containing the details of the supplier to be updated.</param>
        /// <returns>An updated supplier</returns>
        /// <response code="200">Returns the updated supplier</response>
        /// <response code="404">If the supplier does not exist</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Supplier))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateSupplier([FromBody] UpdateSupplierRequest updateSupplier)
        {
            var s = await _mediator.Send(new UpdateSupplierCommand(updateSupplier));

            if (s != null)
            {
                return Ok(s);
            }

            return NotFound();
        }

        /// <summary>
        /// Deletes a supplier by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/suppliers/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the supplier. This ID is used to delete a specific supplier from the database.</param>
        /// <returns>An deleted supplier</returns>
        /// <response code="200">Returns the deleted supplier</response>
        /// <response code="404">If the supplier does not exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Supplier))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteSupplier([FromRoute] long id)
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
