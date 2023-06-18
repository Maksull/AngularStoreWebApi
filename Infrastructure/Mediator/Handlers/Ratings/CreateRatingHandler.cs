using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class CreateRatingHandler : IRequestHandler<CreateRatingCommand, Rating>
    {
        private readonly IRatingService _ratingService;
        private readonly IMetrics _metrics;

        public CreateRatingHandler(IRatingService ratingService, IMetrics metrics)
        {
            _ratingService = ratingService;
            _metrics = metrics;
        }

        public async Task<Rating> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _ratingService.CreateRating(request.Rating, request.User);

            _metrics.Measure.Counter.Increment(MetricsRegistry.CreateRatingCounter);

            return rating;
        }
    }
}
