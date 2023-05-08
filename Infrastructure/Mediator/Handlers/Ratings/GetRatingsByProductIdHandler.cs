using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class GetRatingsByProductIdHandler : IRequestHandler<GetRatingsByProductIdQuery, IEnumerable<Rating>>
    {
        private readonly IRatingService _ratingService;

        public GetRatingsByProductIdHandler(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public Task<IEnumerable<Rating>> Handle(GetRatingsByProductIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_ratingService.GetRatingsByProductId(request.Id));
        }
    }
}
