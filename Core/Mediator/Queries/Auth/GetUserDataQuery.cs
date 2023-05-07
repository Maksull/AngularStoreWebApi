using Core.Contracts.Controllers.Auth;
using MediatR;

namespace Core.Mediator.Queries.Auth
{
    public sealed record GetUserDataQuery(string Username) : IRequest<UserResponse?>;
}
