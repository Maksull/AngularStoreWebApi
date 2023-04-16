using AutoMapper;
using Core.Dto;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration configuration, UserManager<User> userManager, IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(UserDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username!);

            if (user == null)
            {
                return NotFound($"No user with username {request.Username}");
            }
            if (!await _userManager.CheckPasswordAsync(user, request.Password!))
            {
                return BadRequest($"Invalid password");
            }

            string token = await CreateToken(user);

            var refreshToken = GenerateRefreshToken();

            await UpdateUserRefreshTokenAsync(user, refreshToken);

            return Ok(new JwtDto { Jwt = token, RefreshToken = refreshToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            //var user = _mapper.Map<User>(request);

            User user = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
            }
            await _userManager.AddToRoleAsync(user, "Customer");


            var temp = await _userManager.FindByNameAsync("string");
            var roles = await _userManager.GetRolesAsync(temp!);

            return Ok();
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshJwt([FromBody] RefreshToken refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken.Token && u.RefreshTokenExpired == refreshToken.Expired);

            if (user != null)
            {
                string token = await CreateToken(user);

                return Ok(new JwtDto { Jwt = token });
            }

            return Unauthorized();
        }

        [HttpPost("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Validate(JwtDto token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;

            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(token.Jwt, validationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(new JwtDto { Jwt = token.Jwt });
        }



        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:SecurityKey").Value!))
            };
        }

        private async Task<string> CreateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new()
            {
                new(ClaimTypes.Name, user.UserName!),
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
