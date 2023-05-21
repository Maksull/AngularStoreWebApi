using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public sealed class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public IEnumerable<Rating> GetRatings()
        {
            IEnumerable<Rating> ratings = _unitOfWork.Rating.Ratings;

            return ratings;
        }

        public IEnumerable<Rating> GetRatingsByProductId(long id)
        {
            IEnumerable<Rating> ratings = _unitOfWork.Rating.Ratings.Where(r => r.ProductId == id);

            return ratings;
        }

        public IEnumerable<Rating> GetRatingsByUserId(ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.ToString();

            if(userId == null)
            {
                return Enumerable.Empty<Rating>();
            }

            IEnumerable<Rating> ratings = _unitOfWork.Rating.Ratings.Where(r => r.UserId == userId);

            return ratings;
        }

        public async Task<Rating?> GetRating(Guid id)
        {
            string key = $"RatingId={id}";

            var cachedRating = await _cacheService.GetAsync<Rating>(key);

            if (cachedRating != null)
            {
                return cachedRating;
            }

            Rating? rating = await _unitOfWork.Rating.Ratings.FirstOrDefaultAsync(r => r.RatingId == id);

            if (rating != null)
            {
                await _cacheService.SetAsync(key, rating);
            }

            return rating;
        }

        public async Task<Rating> CreateRating(CreateRatingRequest createRating, ClaimsPrincipal user)
        {
            Rating rating = _mapper.Map<Rating>(createRating);
            rating.UserId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.ToString()!;

            await _unitOfWork.Rating.CreateRatingAsync(rating);

            return rating;
        }

        public async Task<Rating?> UpdateRating(UpdateRatingRequest updateRating, ClaimsPrincipal user)
        {
            Rating rating = _mapper.Map<Rating>(updateRating);
            rating.UserId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.ToString()!;

            var t = await _unitOfWork.Rating.Ratings.AsNoTracking().FirstOrDefaultAsync(r => r.RatingId == rating.RatingId);

            if (t != null)
            {
                await _unitOfWork.Rating.UpdateRatingAsync(rating);

                string key = $"RatingId={rating.RatingId}";

                await _cacheService.RemoveAsync(key);

                return rating;
            }

            return null;
        }

        public async Task<Rating?> DeleteRating(Guid id)
        {
            Rating? rating = await _unitOfWork.Rating.Ratings.AsNoTracking().FirstOrDefaultAsync(r => r.RatingId == id);

            if (rating != null)
            {
                string key = $"RatingId={id}";

                await _unitOfWork.Rating.DeleteRatingAsync(rating);

                await _cacheService.RemoveAsync(key);

                return rating;
            }

            return null;
        }

    }
}
