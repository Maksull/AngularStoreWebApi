using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Ratings
{
    public sealed class DeleteRatingHandler : IRequestHandler<DeleteRatingCommand, Rating?>
    {
        private readonly IRatingService _ratingService;

        public DeleteRatingHandler(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public async Task<Rating?> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {
            return await _ratingService.DeleteRating(request.Id);
        }
    }
}
