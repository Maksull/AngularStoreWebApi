using Core.Contracts.Controllers.Auth;
using MediatR;
using System.Security.Claims;

namespace Core.Mediator.Queries.Auth
{
    public sealed record GetUserDataQuery(ClaimsPrincipal User) : IRequest<UserResponse?>;
}
