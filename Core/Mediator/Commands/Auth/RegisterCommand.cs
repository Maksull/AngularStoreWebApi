using Core.Contracts.Controllers.Auth;
using MediatR;

namespace Core.Mediator.Commands.Auth
{
    public sealed record RegisterCommand(RegisterRequest RegisterRequest) : IRequest<IEnumerable<string>>;
}
