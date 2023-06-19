using App.Metrics;
using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class RefreshHandler : IRequestHandler<RefreshCommand, JwtResponse?>
    {
        private readonly IAuthService _authService;
        private readonly IMetrics _metrics;

        public RefreshHandler(IAuthService authService, IMetrics metrics)
        {
            _authService = authService;
            _metrics = metrics;
        }

        public async Task<JwtResponse?> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var newJwtResponse = await _authService.Refresh(request.RefreshTokenRequest);

            _metrics.Measure.Counter.Increment(MetricsRegistry.RefreshCounter);

            return newJwtResponse;
        }
    }
}
