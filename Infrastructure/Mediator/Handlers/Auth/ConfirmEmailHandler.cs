using App.Metrics;
using Core.Mediator.Commands.Auth;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IAuthService _authService;
        private readonly IMetrics _metrics;

        public ConfirmEmailHandler(IAuthService authService, IMetrics metrics)
        {
            _authService = authService;
            _metrics = metrics;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var isEmailConfirmed = await _authService.ConfirmEmail(request.UserId, request.Token);

            _metrics.Measure.Counter.Increment(MetricsRegistry.ConfirmEmailCounter);

            return isEmailConfirmed;
        }
    }
}
