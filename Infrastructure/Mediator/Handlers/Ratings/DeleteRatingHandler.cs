using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class DeleteRatingHandler : IRequestHandler<DeleteRatingCommand, Rating?>
    {
        private readonly IRatingService _ratingService;
        private readonly IMetrics _metrics;

        public DeleteRatingHandler(IRatingService ratingService, IMetrics metrics)
        {
            _ratingService = ratingService;
            _metrics = metrics;
        }

        public async Task<Rating?> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _ratingService.DeleteRating(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.DeleteRatingCounter);

            return rating;
        }
    }
}
