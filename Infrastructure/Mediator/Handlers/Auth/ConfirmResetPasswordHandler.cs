using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class ConfirmResetPasswordHandler : IRequestHandler<ConfirmResetPasswordCommand, IEnumerable<string>>
    {
        private readonly IAuthService _authService;

        public ConfirmResetPasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IEnumerable<string>> Handle(ConfirmResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ConfirmResetPassword(request.UserId, request.Token, request.NewPassword);
        }
    }
}
