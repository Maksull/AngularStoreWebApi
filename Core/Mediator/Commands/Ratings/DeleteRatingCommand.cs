using Core.Entities;
using MediatR;

namespace Core.Mediator.Commands.Ratings
{
    public sealed record DeleteRatingCommand(Guid Id) : IRequest<Rating?>;
}
