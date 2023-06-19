using MediatR;

namespace Core.Mediator.Commands.Auth
{
    public sealed record ConfirmResetPasswordCommand(string UserId, string Token, string NewPassword) : IRequest<IEnumerable<string>>;
}
