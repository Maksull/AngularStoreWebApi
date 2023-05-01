using Core.Contracts.Controllers.Categories;
using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        Task<Category?> GetCategory(long id);
        Task<Category> CreateCategory(CreateCategoryRequest createCategory);
        Task<Category?> UpdateCategory(UpdateCategoryRequest updateCategory);
        Task<Category?> DeleteCategory(long id);
    }
}
