using Core.Contracts.Controllers.Products;
using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Task<Product?> GetProduct(long id);
        Task<Product> CreateProduct(CreateProductRequest productDto);
        Task<Product?> UpdateProduct(UpdateProductRequest productDto);
        Task<Product?> DeleteProduct(long id);

    }
}
