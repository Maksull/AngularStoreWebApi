using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IAuthService _authService;

        public ResetPasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ResetPassword(request.UserId, request.Username);
        }
    }
}
