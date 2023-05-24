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

namespace Infrastructure.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public AuthService(IMapper mapper, IConfiguration configuration, UserManager<User> userManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
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

            string token = await CreateToken(user);

            var refreshToken = GenerateRefreshToken();

            await UpdateUserRefreshTokenAsync(user, refreshToken);

            return new(token, refreshToken);
        }

        public async Task<List<string>> Register(RegisterRequest register)
        {
            User user = _mapper.Map<User>(register);

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                return new List<string>();
            }
            List<string> errors = new();

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
