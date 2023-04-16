using AutoMapper;
using Core.Dto;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
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
            if (_unitOfWork.Product.Products != null)
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

        public async Task<Product> CreateProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            await _s3Service.AddImageToBucket(productDto.Img!, product.Images);

            await _unitOfWork.Product.CreateProductAsync(product);

            return product;
        }

        public async Task<Product?> UpdateProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            if (await _unitOfWork.Product.Products.ContainsAsync(product))
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


                await _unitOfWork.Product.UpdateProductAsync(product);

                return product;
            }

            return null;
        }

        public async Task<Product?> DeleteProduct(long id)
        {
            if (_unitOfWork.Product.Products != null)
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
            if (_unitOfWork.Product.Products != null)
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
