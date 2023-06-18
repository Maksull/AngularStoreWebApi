using App.Metrics;
using Core.Contracts.Controllers.Auth;
using Core.Mediator.Queries.Auth;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class GetUserDataHandler : IRequestHandler<GetUserDataQuery, UserResponse?>
    {
        private readonly IAuthService _authService;
        private readonly IMetrics _metrics;

        public GetUserDataHandler(IAuthService authService, IMetrics metrics)
        {
            _authService = authService;
            _metrics = metrics;
        }

        public async Task<UserResponse?> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
        {
            var userResponse = await _authService.GetUserData(request.User);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetUserDataCounter);

            return userResponse;
        }
    }
}
