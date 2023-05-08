using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class CreateRatingHandler : IRequestHandler<CreateRatingCommand, Rating>
    {
        private readonly IRatingService _ratingService;

        public CreateRatingHandler(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public async Task<Rating> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            return await _ratingService.CreateRating(request.Rating, request.User);
        }
    }
}
