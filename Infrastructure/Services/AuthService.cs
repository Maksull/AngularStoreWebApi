using Core.Contracts.Controllers.Auth;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;

namespace Infrastructure.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public AuthService(IMapper mapper, IConfiguration configuration, UserManager<User> userManager, IEmailService emailService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<JwtResponse?> Login(LoginRequest login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);

            if (user == null)
            {
                return null;
            }
            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                return null;
            }

            var result = await _userManager.IsEmailConfirmedAsync(user);
            if (result == false)
            {
                return null;
            }

            string token = await CreateToken(user);

            var refreshToken = GenerateRefreshToken();

            await UpdateUserRefreshTokenAsync(user, refreshToken);

            return new(token, refreshToken);
        }

        public async Task<IEnumerable<string>> Register(RegisterRequest register)
        {
            User user = _mapper.Map<User>(register);

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //var confirmationLink = $"https://localhost:44301/api/auth/confirmEmail?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";

                var confirmationLink = $"{_configuration["AngularStore:Url"]}/login/{user.Id}/{HttpUtility.UrlEncode(token)}";


                string emailBody = $"Please confirm your email by clicking the link below:<br/><br/><a href=\"{confirmationLink}\">Confirm Email</a>";

                _emailService.Send(new Core.Contracts.Services.EmailService.EmailRequest(user.Email!, "Email confirmation", emailBody));

                return Enumerable.Empty<string>();
            }

            List<string> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            return errors;
        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ResetPassword(string? userId, string? username)
        {
            User? user = null;
            if (userId is not null)
            {
                user = await _userManager.FindByIdAsync(userId);
            }
            if (username is not null)
            {
                user = await _userManager.FindByNameAsync(username);
            }


            if (user is null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //var confirmationLink = $"https://localhost:44301/api/auth/confirmResetPassword?userId={user.Id}&token={HttpUtility.UrlEncode(token)}&newPassword=NewPassword123$";

            var confirmationLink = $"{_configuration["AngularStore:Url"]}/login/resetPassword/{user.Id}/{HttpUtility.UrlEncode(token)}";

            string emailBody = $"Please confirm your password by clicking the link below:<br/><br/><a href=\"{confirmationLink}\">Reset Password</a>";

            _emailService.Send(new Core.Contracts.Services.EmailService.EmailRequest(user.Email!, "Password reset", emailBody));

            return true;
        }

        public async Task<IEnumerable<string>> ConfirmResetPassword(string userId, string token, string newPassword)
        {
            List<string> errors = new();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                errors.Add("The user does not exist");

                return errors;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return Enumerable.Empty<string>();
            }

            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            return errors;
        }

        public async Task<JwtResponse?> Refresh(RefreshTokenRequest request)
        {
            RefreshToken refreshToken = _mapper.Map<RefreshToken>(request);

            if (refreshToken.Expired > DateTime.UtcNow)
            {
                var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken.Token && u.RefreshTokenExpired == refreshToken.Expired);

                if (user != null)
                {
                    string token = await CreateToken(user);

                    return new(token, refreshToken);
                }
            }

            return null;
        }

        public async Task<UserResponse?> GetUserData(ClaimsPrincipal user)
        {
            var id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.ToString()!;
            var u = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (u != null)
            {
                var result = _mapper.Map<UserResponse>(u);

                return result;
            }

            return null;
        }


        private async Task<string> CreateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user) ?? new List<string>();
            List<Claim> claims = new()
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Expiration, DateTime.Now.ToString()),

            };
            List<Claim> rolesList = new();

            foreach (var role in roles)
            {
                rolesList.Add(new(ClaimTypes.Role, role));
            }

            claims.AddRange(rolesList);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:SecurityKey").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration.GetSection("JwtSettings:ExpiresInMinutes").Value!)),
                signingCredentials: credentials
                );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private static RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expired = DateTime.Now.AddHours(5),
            };

            return refreshToken;
        }
        private async Task UpdateUserRefreshTokenAsync(User user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpired = refreshToken.Expired;

            await _userManager.UpdateAsync(user);
        }
    }
}
