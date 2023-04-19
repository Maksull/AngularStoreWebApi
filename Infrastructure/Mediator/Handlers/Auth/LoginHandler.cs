using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class LoginHandler : IRequestHandler<LoginCommand, JwtResponse?>
    {
        private readonly IAuthService _authService;

        public LoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<JwtResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.Login(request.LoginRequest);
        }
    }
}
