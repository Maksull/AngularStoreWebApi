using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Auth
{
    public sealed class RegisterHandler : IRequestHandler<RegisterCommand, IEnumerable<string>>
    {
        private readonly IAuthService _authService;

        public RegisterHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IEnumerable<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.Register(request.RegisterRequest);
        }
    }
}
