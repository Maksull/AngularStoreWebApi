using App.Metrics;
using Core.Mediator.Commands.Auth;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IAuthService _authService;
        private readonly IMetrics _metrics;

        public ResetPasswordHandler(IAuthService authService, IMetrics metrics)
        {
            _authService = authService;
            _metrics = metrics;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var isResetPasswordRequestSent = await _authService.ResetPassword(request.UserId, request.Username);

            _metrics.Measure.Counter.Increment(MetricsRegistry.ResetPasswordCounter);

            return isResetPasswordRequestSent;
        }
    }
}
