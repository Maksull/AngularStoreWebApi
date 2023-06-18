using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class GetRatingsByProductIdHandler : IRequestHandler<GetRatingsByProductIdQuery, IEnumerable<Rating>>
    {
        private readonly IRatingService _ratingService;
        private readonly IMetrics _metrics;

        public GetRatingsByProductIdHandler(IRatingService ratingService, IMetrics metrics)
        {
            _ratingService = ratingService;
            _metrics = metrics;
        }

        public Task<IEnumerable<Rating>> Handle(GetRatingsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var ratings = _ratingService.GetRatingsByProductId(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetRatingByProductIdCounter);

            return Task.FromResult(ratings);
        }
    }
}
