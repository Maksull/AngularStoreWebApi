using AutoMapper;
using Core.Dto;
using Core.Entities;

namespace Infrastructure.AutoMapper
{
    public sealed class ProductDtoConverter : ITypeConverter<ProductDto, Product>
    {
        public Product Convert(ProductDto source, Product destination, ResolutionContext context)
        {
            if (source.Img != null)
            {
                return new()
                {
                    ProductId = source.ProductId,
                    Name = source.Name,
                    Description = source.Description,
                    Price = source.Price,
                    CategoryId = source.CategoryId,
                    SupplierId = source.SupplierId,
                    Images = $"{source.Name}/{source.Img.FileName}"
                };
            }
            else
            {
                return new()
                {
                    ProductId = source.ProductId,
                    Name = source.Name,
                    Description = source.Description,
                    Price = source.Price,
                    CategoryId = source.CategoryId,
                    SupplierId = source.SupplierId
                };
            }
        }
    }
}
