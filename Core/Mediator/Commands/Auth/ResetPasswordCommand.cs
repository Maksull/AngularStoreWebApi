using MediatR;

namespace Core.Mediator.Commands.Auth
{
    public sealed record ResetPasswordCommand(string? UserId, string? Username) : IRequest<bool>;
}
