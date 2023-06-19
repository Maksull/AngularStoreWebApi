using Core.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        IQueryable<Rating> Ratings { get; }

        Task CreateRatingAsync(Rating rating);
        Task DeleteRatingAsync(Rating rating);
        Task UpdateRatingAsync(Rating rating);
    }
}
