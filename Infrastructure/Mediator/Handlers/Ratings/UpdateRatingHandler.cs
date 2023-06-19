using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class UpdateRatingHandler : IRequestHandler<UpdateRatingCommand, Rating?>
    {
        private readonly IRatingService _ratingService;
        private readonly IMetrics _metrics;

        public UpdateRatingHandler(IRatingService ratingService, IMetrics metrics)
        {
            _ratingService = ratingService;
            _metrics = metrics;
        }

        public async Task<Rating?> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _ratingService.UpdateRating(request.Rating, request.User);

            _metrics.Measure.Counter.Increment(MetricsRegistry.UpdateRatingCounter);

            return rating;
        }
    }
}
