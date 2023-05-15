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
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a list of products.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/products
        ///     
        /// </remarks>
        /// <returns>Returns the list of products</returns>
        /// <response code="200">Returns the list of products</response>
        /// <response code="404">If the products do not exist</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _mediator.Send(new GetProductsQuery());

            if (products.Any())
            {
                return Ok(products);
            }

            return NotFound();
        }

        /// <summary>
        /// Gets a product by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/products/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the product. This ID is used to retrieve a specific product from the database.</param>
        /// <returns>A product</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="404">If the product does not exist</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetProduct([FromRoute] long id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        /// <summary>
        /// Creates a product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/products
        ///     
        /// </remarks>
        /// <param name="createProduct">The product object containing the details of the product to be created.</param>
        /// <returns>A newly created product</returns>
        /// <response code="200">Returns the newly created product</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest createProduct)
        {
            var product = await _mediator.Send(new CreateProductCommand(createProduct));

            return Ok(product);
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/products
        ///     
        /// </remarks>
        /// <param name="updateProduct">The product object containing the details of the product to be updated.</param>
        /// <returns>An updated product</returns>
        /// <response code="200">Returns the updated product</response>
        /// <response code="404">If the product does not exist</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductRequest updateProduct)
        {
            var product = await _mediator.Send(new UpdateProductCommand(updateProduct));

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        /// <summary>
        /// Deletes a product by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/products/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the product. This ID is used to delete a specific product from the database.</param>
        /// <returns>An deleted product</returns>
        /// <response code="200">Returns the deleted product</response>
        /// <response code="404">If the product does not exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            var product = await _mediator.Send(new DeleteProductCommand(id));

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }
    }
}
