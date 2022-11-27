using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Models.Repository;
using WebApi.Services.S3Service;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IS3Service _s3Service;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IS3Service s3Service, IMapper mapper)
        {
            _repository = repository;
            _s3Service = s3Service;
            _mapper = mapper;
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
            if (_repository.Products != null)
            {
                Product? p = await _repository.Products.Include(p => p.Category).Include(p => p.Supplier).FirstOrDefaultAsync(p => p.ProductId == id);

                if (p != null)
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
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            await _s3Service.AddImageToBucket(productDto.Img!, product.Images);

            await _repository.CreateProductAsync(product);
            return Ok(product);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            if (await _repository.Products.ContainsAsync(product))
            {
                if (productDto.Img != null)
                {
                    await _s3Service.DeleteImageFromBucket(await GetProductImagePath(product.ProductId));
                    await _s3Service.AddImageToBucket(productDto.Img!, product.Images);
                }
                else
                {
                    product.Images = await GetProductImagePath(product.ProductId);
                }


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
            if (_repository.Products != null)
            {
                Product? p = await _repository.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if (p != null)
                {
                    await _s3Service.DeleteImageFromBucket(await GetProductImagePath(id));
                    await _repository.DeleteProductAsync(p);
                    return Ok(p);
                }
            }
            return NotFound();
        }




        private async Task<string> GetProductImagePath(long id)
        {
            if (_repository.Products != null)
            {
                Product? p = await _repository.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);

                if (p != null)
                {
                    return p.Images;
                }
            }
            return string.Empty;
        }
    }
}
