using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class RefreshHandler : IRequestHandler<RefreshCommand, JwtResponse?>
    {
        private readonly IAuthService _authService;

        public RefreshHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<JwtResponse?> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            return await _authService.Refresh(request.RefreshTokenRequest);
        }
    }
}
