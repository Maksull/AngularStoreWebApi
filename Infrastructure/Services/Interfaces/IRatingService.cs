using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using System.Security.Claims;

namespace Infrastructure.Services.Interfaces
{
    public interface IRatingService
    {
        IEnumerable<Rating> GetRatings();
        IEnumerable<Rating> GetRatingsByProductId(long id);
        Task<Rating?> GetRating(Guid id);
        Task<Rating> CreateRating(CreateRatingRequest createRating, ClaimsPrincipal user);
        Task<Rating?> UpdateRating(UpdateRatingRequest updateRating, ClaimsPrincipal user);
        Task<Rating?> DeleteRating(Guid id);
    }
}
