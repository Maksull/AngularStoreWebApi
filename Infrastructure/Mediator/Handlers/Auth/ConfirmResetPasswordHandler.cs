using App.Metrics;
using Core.Mediator.Commands.Auth;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class ConfirmResetPasswordHandler : IRequestHandler<ConfirmResetPasswordCommand, IEnumerable<string>>
    {
        private readonly IAuthService _authService;
        private readonly IMetrics _metrics;

        public ConfirmResetPasswordHandler(IAuthService authService, IMetrics metrics)
        {
            _authService = authService;
            _metrics = metrics;
        }

        public async Task<IEnumerable<string>> Handle(ConfirmResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var isPasswordResetConfirmed = await _authService.ConfirmResetPassword(request.UserId, request.Token, request.NewPassword);

            _metrics.Measure.Counter.Increment(MetricsRegistry.ConfirmResetPasswordCounter);

            return isPasswordResetConfirmed;
        }
    }
}
