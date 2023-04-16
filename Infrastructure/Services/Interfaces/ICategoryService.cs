using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category>? GetCategories();
        Task<Category?> GetCategory(long id);
        Task<Category> CreateCategory(Category category);
        Task<Category?> UpdateCategory(Category category);
        Task<Category?> DeleteCategory(long id);
    }
}
