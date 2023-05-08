using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public sealed class RatingRepository : IRatingRepository
    {
        private readonly ApiDataContext _context;

        public RatingRepository(ApiDataContext context)
        {
            _context = context;
        }

        public IQueryable<Rating> Ratings => _context.Ratings;

        public async Task CreateRatingAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRatingAsync(Rating rating)
        {
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRatingAsync(Rating rating)
        {
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();
        }
    }
}
