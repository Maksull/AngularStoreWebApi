using Core.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category>? GetCategories()
        {
            if (_unitOfWork.Category.Categories != null)
            {
                IEnumerable<Category> categories = _unitOfWork.Category.Categories.Include(c => c.Products);

                foreach (var c in categories)
                {
                    foreach (var p in c.Products!)
                    {
                        p.Category = null;
                    }
                }

                return categories;
            }

            return null;
        }

        public async Task<Category?> GetCategory(long id)
        {
            if (_unitOfWork.Category.Categories != null)
            {
                Category? category = await _unitOfWork.Category.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryId == id);

                if (category != null)
                {
                    foreach (var p in category.Products!)
                    {
                        p.Category = null;
                    }

                    return category;
                }
            }

            return null;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            await _unitOfWork.Category.CreateCategoryAsync(category);

            return category;
        }
        public async Task<Category?> UpdateCategory(Category category)
        {
            if (await _unitOfWork.Category.Categories.ContainsAsync(category))
            {
                await _unitOfWork.Category.UpdateCategoryAsync(category);

                return category;
            }

            return null;
        }

        public async Task<Category?> DeleteCategory(long id)
        {
            if (_unitOfWork.Category.Categories != null)
            {
                Category? category = await _unitOfWork.Category.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

                if (category != null)
                {
                    await _unitOfWork.Category.DeleteCategoryAsync(category);
                    return category;
                }
            }

            return null;
        }

    }
}
