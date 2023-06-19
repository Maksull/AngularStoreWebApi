using App.Metrics;
using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class LoginHandler : IRequestHandler<LoginCommand, JwtResponse?>
    {
        private readonly IAuthService _authService;
        private readonly IMetrics _metrics;

        public LoginHandler(IAuthService authService, IMetrics metrics
            )
        {
            _authService = authService;
            _metrics = metrics;
        }

        public async Task<JwtResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var jwtResponse = await _authService.Login(request.LoginRequest);

            _metrics.Measure.Counter.Increment(MetricsRegistry.LoginCounter);

            return jwtResponse;
        }
    }
}
