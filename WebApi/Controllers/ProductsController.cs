using Core.Contracts.Controllers.Products;
using Core.Entities;
using Core.Mediator.Commands.Products;
using Core.Mediator.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _mediator.Send(new GetProductsQuery());

                if (products.Any())
                {
                    return Ok(products);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetProduct(long id)
        {
            try
            {
                var product = await _mediator.Send(new GetProductByIdQuery(id));

                if (product != null)
                {
                    return Ok(product);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest createProduct)
        {
            try
            {
                var product = await _mediator.Send(new CreateProductCommand(createProduct));

                return Ok(product);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductRequest updateProduct)
        {
            try
            {
                var product = await _mediator.Send(new UpdateProductCommand(updateProduct));

                if (product != null)
                {
                    return Ok(product);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
                var product = await _mediator.Send(new DeleteProductCommand(id));

                if (product != null)
                {
                    return Ok(product);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
