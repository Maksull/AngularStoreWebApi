using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class GetRatingsByUserIdHandler : IRequestHandler<GetRatingsByUserIdQuery, IEnumerable<Rating>>
    {
        private readonly IRatingService _ratingService;
        private readonly IMetrics _metrics;

        public GetRatingsByUserIdHandler(IRatingService ratingService, IMetrics metrics)
        {
            _ratingService = ratingService;
            _metrics = metrics;
        }

        public Task<IEnumerable<Rating>> Handle(GetRatingsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var ratings = _ratingService.GetRatingsByUserId(request.User);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetRatingByUserIdCounter);

            return Task.FromResult(ratings);
        }
    }
}
