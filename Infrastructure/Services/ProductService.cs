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

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IS3Service s3Service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _s3Service = s3Service;
        }

        public IEnumerable<Product> GetProducts()
        {
            if (_unitOfWork.Product.Products.Any())
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

            return Enumerable.Empty<Product>();
        }

        public async Task<Product?> GetProduct(long id)
        {
            if (_unitOfWork.Product.Products.Any())
            {
                var product = await _unitOfWork.Product.Products
                                .Include(p => p.Category)
                                .Include(p => p.Supplier)
                                .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product != null)
                {
                    product.Category!.Products = null;
                    product.Supplier!.Products = null;
                }

                return product;
            }

            return null;
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

            if (await _unitOfWork.Product.Products.ContainsAsync(product))
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
            if (_unitOfWork.Product.Products.Any())
            {
                Product? product = await _unitOfWork.Product.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if (product != null)
                {
                    await _s3Service.DeleteImageFromBucket(await GetProductImagePath(id));
                    await _unitOfWork.Product.DeleteProductAsync(product);

                    return product;
                }
            }

            return null;
        }

        private async Task<string> GetProductImagePath(long id)
        {
            if (_unitOfWork.Product.Products.Any())
            {
                Product? p = await _unitOfWork.Product.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);

                if (p != null)
                {
                    return p.Images;
                }
            }
            return string.Empty;
        }
    }
}
