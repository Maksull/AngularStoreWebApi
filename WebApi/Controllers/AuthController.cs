using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Models.Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
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
            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(IdentityUser user)
        {
            List<Claim> claims = new()
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Role, "Admin")
            };

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
    }
}
