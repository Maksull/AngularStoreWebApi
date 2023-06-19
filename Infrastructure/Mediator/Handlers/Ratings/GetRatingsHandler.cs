using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class GetRatingsHandler : IRequestHandler<GetRatingsQuery, IEnumerable<Rating>>
    {
        private readonly IRatingService _ratingService;
        private readonly IMetrics _metrics;

        public GetRatingsHandler(IRatingService ratingService, IMetrics metrics)
        {
            _ratingService = ratingService;
            _metrics = metrics;
        }

        public Task<IEnumerable<Rating>> Handle(GetRatingsQuery request, CancellationToken cancellationToken)
        {
            var ratings = _ratingService.GetRatings();

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetRatingsCounter);

            return Task.FromResult(ratings);
        }
    }
}
