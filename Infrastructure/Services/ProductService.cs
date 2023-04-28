using Core.Contracts.Controllers.Products;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IS3Service _s3Service;
        private readonly ICacheService _cacheService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IS3Service s3Service, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _s3Service = s3Service;
            _cacheService = cacheService;
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = _unitOfWork.Product.Products
                            .Include(p => p.Category)
                            .Include(p => p.Supplier);

            foreach (var p in products)
            {
                p.Category!.Products = null;
                p.Supplier!.Products = null;
            }

            return products;
        }

        public async Task<Product?> GetProduct(long id)
        {
            string key = $"ProductId={id}";

            var cachedProduct = await _cacheService.GetAsync<Product>(key);

            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            Product? product = await _unitOfWork.Product.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product != null)
            {
                product.Category!.Products = null;
                product.Supplier!.Products = null;

                await _cacheService.SetAsync(key, product);
            }

            return product;
        }

        public async Task<Product> CreateProduct(CreateProductRequest createProduct)
        {
            Product product = _mapper.Map<Product>(createProduct);

            await _s3Service.AddImageToBucket(createProduct.Img!, product.Images);

            await _unitOfWork.Product.CreateProductAsync(product);

            return product;
        }

        public async Task<Product?> UpdateProduct(UpdateProductRequest updateProduct)
        {
            Product product = _mapper.Map<Product>(updateProduct);

            var t = await _unitOfWork.Product.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

            if (t != null)
            {
                if (updateProduct.Img != null)
                {
                    await _s3Service.DeleteImageFromBucket(await GetProductImagePath(product.ProductId));
                    await _s3Service.AddImageToBucket(updateProduct.Img!, product.Images);
                }
                else
                {
                    product.Images = await GetProductImagePath(product.ProductId);
                }


                await _unitOfWork.Product.UpdateProductAsync(product);

                return product;
            }

            return null;
        }

        public async Task<Product?> DeleteProduct(long id)
        {
            Product? product = await _unitOfWork.Product.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product != null)
            {
                string key = $"ProductId={id}";

                await _s3Service.DeleteImageFromBucket(await GetProductImagePath(id));
                await _unitOfWork.Product.DeleteProductAsync(product);

                await _cacheService.RemoveAsync(key);
            }

            return product;
        }

        private async Task<string> GetProductImagePath(long id)
        {
            Product? p = await _unitOfWork.Product.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);

            if (p != null)
            {
                return p.Images;
            }

            return string.Empty;
        }
    }
}
