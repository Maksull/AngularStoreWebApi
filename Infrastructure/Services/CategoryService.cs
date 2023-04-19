using Core.Contracts.Controllers.Categories;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public IEnumerable<Category> GetCategories()
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

        public async Task<Category?> GetCategory(long id)
        {
            string key = $"CategoryId={id}";

            var cachedCategory = await _cacheService.GetAsync<Category>(key);

            if (cachedCategory != null)
            {
                return cachedCategory;
            }

            Category? category = await _unitOfWork.Category.Categories.Include(s => s.Products).FirstOrDefaultAsync(s => s.CategoryId == id);

            if (category != null)
            {
                foreach (var p in category.Products!)
                {
                    p.Category = null;
                }

                await _cacheService.SetAsync(key, category);
            }

            return category;
        }

        public async Task<Category> CreateCategory(CreateCategoryRequest createCategory)
        {
            Category category = _mapper.Map<Category>(createCategory);

            await _unitOfWork.Category.CreateCategoryAsync(category);

            return category;
        }

        public async Task<Category?> UpdateCategory(UpdateCategoryRequest updateCategory)
        {
            Category category = _mapper.Map<Category>(updateCategory);

            if (await _unitOfWork.Category.Categories.ContainsAsync(category))
            {
                await _unitOfWork.Category.UpdateCategoryAsync(category);

                return category;
            }

            return null;
        }

        public async Task<Category?> DeleteCategory(long id)
        {
            Category? category = await _unitOfWork.Category.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category != null)
            {
                string key = $"CategoryId={id}";

                await _unitOfWork.Category.DeleteCategoryAsync(category);

                await _cacheService.RemoveAsync(key);

                return category;
            }

            return null;
        }

    }
}
