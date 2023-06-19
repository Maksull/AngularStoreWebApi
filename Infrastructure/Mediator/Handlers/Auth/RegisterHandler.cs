using App.Metrics;
using Core.Mediator.Commands.Auth;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class RegisterHandler : IRequestHandler<RegisterCommand, IEnumerable<string>>
    {
        private readonly IAuthService _authService;
        private readonly IMetrics _metrics;

        public RegisterHandler(IAuthService authService, IMetrics metrics)
        {
            _authService = authService;
            _metrics = metrics;
        }

        public async Task<IEnumerable<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var errors = await _authService.Register(request.RegisterRequest);

            _metrics.Measure.Counter.Increment(MetricsRegistry.RegisterCounter);

            return errors;
        }
    }
}
