using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProducts()
        {
            if (_repository.Products != null)
            {
                IEnumerable<Product> products = _repository.Products.Include(p => p.Category).Include(p => p.Supplier);

                foreach (var p in products)
                {
                    p.Category!.Products = null;
                    p.Supplier!.Products = null;
                }

                return Ok(products);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(long id)
        {
            if(_repository.Products != null)
            {
                Product? p = await _repository.Products.Include(p => p.Category).Include(p => p.Supplier).FirstOrDefaultAsync(p => p.ProductId == id);

                if(p != null)
                {
                    p.Category!.Products = null;
                    p.Supplier!.Products = null;
                    return Ok(p);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            product.Category = null;
            product.Supplier = null;
            await _repository.CreateProductAsync(product);
            return Ok(product);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if(await _repository.Products.ContainsAsync(product))
            {
                product.Category = null;
                product.Supplier = null;
                await _repository.UpdateProductAsync(product);
                return Ok(product);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            if(_repository.Products != null)
            {
                Product? p = await _repository.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if(p != null)
                {
                    await _repository.DeleteProductAsync(p);
                    return Ok(p);
                }
            }
            
            return NotFound();
        }
    }
}
