using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Ratings;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class GetRatingByIdHandler : IRequestHandler<GetRatingByIdQuery, Rating?>
    {
        private readonly IRatingService _ratingService;
        private readonly IMetrics _metrics;

        public GetRatingByIdHandler(IRatingService ratingService, IMetrics metrics)
        {
            _ratingService = ratingService;
            _metrics = metrics;
        }

        public async Task<Rating?> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
        {
            var rating = await _ratingService.GetRating(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetRatingByIdCounter);

            return rating;
        }
    }
}
