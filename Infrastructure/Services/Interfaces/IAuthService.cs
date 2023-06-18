using Core.Contracts.Controllers.Auth;
using System.Security.Claims;

namespace Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<JwtResponse?> Login(LoginRequest login);
        Task<IEnumerable<string>> Register(RegisterRequest register);
        Task<bool> ConfirmEmail(string userId, string token);
        Task<JwtResponse?> Refresh(RefreshTokenRequest request);
        Task<UserResponse?> GetUserData(ClaimsPrincipal user);
        Task<bool> ResetPassword(string? userId, string? username);
        Task<IEnumerable<string>> ConfirmResetPassword(string userId, string token, string newPassword);

    }
}
