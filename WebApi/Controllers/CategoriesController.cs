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

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetCategory(long id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (category != null)
            {
                return Ok(category);
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest createCategory)
        {
            var c = await _mediator.Send(new CreateCategoryCommand(createCategory));

            return Ok(c);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest updateCategory)
        {
            var c = await _mediator.Send(new UpdateCategoryCommand(updateCategory));

            if (c != null)
            {
                return Ok(c);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteCategory(long id)
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
