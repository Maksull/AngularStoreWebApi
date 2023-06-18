using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IAuthService _authService;

        public ConfirmEmailHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ConfirmEmail(request.UserId, request.Token);
        }
    }
}
