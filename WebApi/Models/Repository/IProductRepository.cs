namespace WebApi.Models.Repository
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        Task SaveProductAsync(Product product);
        Task CreateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
