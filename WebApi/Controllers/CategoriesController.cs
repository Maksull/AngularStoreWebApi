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
    public sealed class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategories()
        {
            if(_repository.Categories != null)
            {
                IEnumerable<Category> categories = _repository.Categories.Include(c => c.Products);

                foreach(var c in categories)
                {
                    foreach(var p in c.Products!)
                    {
                        p.Category = null;
                    }
                }

                return Ok(categories);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(long id)
        {
            if(_repository.Categories != null)
            {
                Category? c = await _repository.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryId == id);

                if(c != null)
                {
                    foreach (var p in c.Products!)
                    {
                        p.Category = null;
                    }
                    return Ok(c);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            await _repository.CreateCategoryAsync(category);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if(await _repository.Categories.ContainsAsync(category))
            {
                await _repository.UpdateCategoryAsync(category);
                return Ok(category);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            if(_repository.Categories != null)
            {
                Category? c = await _repository.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

                if(c != null)
                {
                    await _repository.DeleteCategoryAsync(c);
                    return Ok(c);
                }
            }
            return NotFound();
        }
    }
}
