using WebApi.Models.Database;

namespace WebApi.Models.Repository
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly ApiDataContext _context;

        public CategoryRepository(ApiDataContext context)
        {
            _context = context;
        }

        public IQueryable<Category> Categories => _context.Categories;

        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task SaveCategoryAsync(Category category)
        {
            await _context.SaveChangesAsync();
        }
    }
}
