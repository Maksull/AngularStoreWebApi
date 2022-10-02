namespace WebApi.Models.Repository
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        Task CreateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task UpdateProductAsync(Product product);
    }
}
