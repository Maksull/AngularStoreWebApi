using WebApi.Models.Database;

namespace WebApi.Models.Repository
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly ApiDataContext _context;

        public ProductRepository(ApiDataContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products;

        public async Task CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task SaveProductAsync(Product product)
        {
            await _context.SaveChangesAsync();
        }
    }
}
