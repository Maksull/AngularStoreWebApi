using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using MediatR;
using System.Security.Claims;

namespace Core.Mediator.Commands.Ratings
{
    public sealed record UpdateRatingCommand(UpdateRatingRequest Rating, ClaimsPrincipal User) : IRequest<Rating?>;
}
