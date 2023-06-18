using MediatR;

namespace Core.Mediator.Commands.Auth
{
    public sealed record ConfirmEmailCommand(string UserId, string Token) : IRequest<bool>;
}
