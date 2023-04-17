using Core.Contracts.Controllers.Auth;

namespace Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<JwtResponse?> Login(LoginRequest login);
        Task<bool> Register(RegisterRequest register);
        Task<JwtResponse?> Refresh(RefreshTokenRequest request);
    }
}
