using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class UpdateRatingHandler : IRequestHandler<UpdateRatingCommand, Rating?>
    {
        private readonly IRatingService _ratingService;

        public UpdateRatingHandler(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public async Task<Rating?> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            return await _ratingService.UpdateRating(request.Rating, request.User);
        }
    }
}
