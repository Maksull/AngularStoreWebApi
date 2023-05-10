using Core.Entities;
using MediatR;
using System.Security.Claims;

namespace Core.Mediator.Queries.Ratings
{
    public sealed record GetRatingsByUserIdQuery(ClaimsPrincipal User) : IRequest<IEnumerable<Rating>>;
}
