using Core.Contracts.Controllers.Categories;
using Core.Entities;
using Core.Mediator.Commands.Categories;
using Core.Mediator.Queries.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public sealed class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a list of categories.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/categories
        ///     
        /// </remarks>
        /// <returns>Returns the list of categories</returns>
        /// <response code="200">Returns the list of categories</response>
        /// <response code="404">If the categories do not exist</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());

            if (categories.Any())
            {
                return Ok(categories);
            }

            return NotFound();
        }

        /// <summary>
        /// Gets a category by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/categories/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the category. This ID is used to retrieve a specific category from the database.</param>
        /// <returns>A category</returns>
        /// <response code="200">Returns the category</response>
        /// <response code="404">If the category does not exist</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetCategory([FromRoute] long id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (category != null)
            {
                return Ok(category);
            }

            return NotFound();
        }

        /// <summary>
        /// Creates a category.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/categories
        ///     {        
        ///       "name": "Bikes",
        ///     }
        /// </remarks>
        /// <param name="createCategory">The category object containing the details of the category to be created.</param>
        /// <returns>A newly created category</returns>
        /// <response code="200">Returns the newly created category</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest createCategory)
        {
            var c = await _mediator.Send(new CreateCategoryCommand(createCategory));

            return Ok(c);
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/categories
        ///     {        
        ///       "categoryId": 1,
        ///       "name": "Bikes",
        ///     }
        /// </remarks>
        /// <param name="updateCategory">The category object containing the details of the category to be updated.</param>
        /// <returns>An updated category</returns>
        /// <response code="200">Returns the updated category</response>
        /// <response code="404">If the category does not exist</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest updateCategory)
        {
            var c = await _mediator.Send(new UpdateCategoryCommand(updateCategory));

            if (c != null)
            {
                return Ok(c);
            }

            return NotFound();
        }

        /// <summary>
        /// Deletes a category by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/categories/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the category. This ID is used to delete a specific category from the database.</param>
        /// <returns>An deleted category</returns>
        /// <response code="200">Returns the deleted category</response>
        /// <response code="404">If the category does not exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id)
        {
            var category = await _mediator.Send(new DeleteCategoryCommand(id));

            if (category != null)
            {
                return Ok(category);
            }

            return NotFound();
        }
    }
}
