using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class GetRatingByIdHandler : IRequestHandler<GetRatingByIdQuery, Rating?>
    {
        private readonly IRatingService _ratingService;

        public GetRatingByIdHandler(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public async Task<Rating?> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
        {
            return await _ratingService.GetRating(request.Id);
        }
    }
}
