using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using MediatR;
using System.Security.Claims;

namespace Core.Mediator.Commands.Ratings
{
    public sealed record CreateRatingCommand(CreateRatingRequest Rating, ClaimsPrincipal User) : IRequest<Rating>;
}
