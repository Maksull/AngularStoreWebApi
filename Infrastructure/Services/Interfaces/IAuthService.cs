using Core.Contracts.Controllers.Auth;
using System.Security.Claims;

namespace Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<JwtResponse?> Login(LoginRequest login);
        Task<bool> Register(RegisterRequest register);
        Task<JwtResponse?> Refresh(RefreshTokenRequest request);
        Task<UserResponse?> GetUserData(ClaimsPrincipal user);
    }
}
