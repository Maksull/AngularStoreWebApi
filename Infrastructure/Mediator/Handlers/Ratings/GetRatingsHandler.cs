using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class GetRatingsHandler : IRequestHandler<GetRatingsQuery, IEnumerable<Rating>>
    {
        private readonly IRatingService _ratingService;

        public GetRatingsHandler(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public Task<IEnumerable<Rating>> Handle(GetRatingsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_ratingService.GetRatings());
        }
    }
}
