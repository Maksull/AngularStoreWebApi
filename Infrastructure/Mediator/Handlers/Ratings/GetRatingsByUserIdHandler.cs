using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class GetRatingsByUserIdHandler : IRequestHandler<GetRatingsByUserIdQuery, IEnumerable<Rating>>
    {
        private readonly IRatingService _ratingService;

        public GetRatingsByUserIdHandler(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        public Task<IEnumerable<Rating>> Handle(GetRatingsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_ratingService.GetRatingsByUserId(request.User));
        }
    }
}
