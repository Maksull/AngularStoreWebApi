using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Core.Mediator.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _mediator.Send(new LoginCommand(request));

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Invalid credentials");
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _mediator.Send(new RegisterCommand(request));

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        public async Task<IActionResult> RefreshJwt(RefreshTokenRequest refreshToken)
        {
            var result = await _mediator.Send(new RefreshCommand(refreshToken));

            if (result != null)
            {
                return Ok(result);
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpGet("userData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetUserData()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.ToString()!;

            var result = await _mediator.Send(new GetUserDataQuery(User));

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("protected")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
        public IActionResult Protected()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adminProtected")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
        public IActionResult AdminProtected()
        {
            return Ok();
        }
    }
}
