namespace WebApi.Models.Repository
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }

        Task SaveCategoryAsync(Category category);
        Task CreateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
