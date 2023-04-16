using Core.Dto;
using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Task<Product?> GetProduct(long id);
        Task<Product> CreateProduct(ProductDto productDto);
        Task<Product?> UpdateProduct(ProductDto productDto);
        Task<Product?> DeleteProduct(long id);

    }
}
