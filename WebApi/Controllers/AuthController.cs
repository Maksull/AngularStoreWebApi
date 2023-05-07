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
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _mediator.Send(new LoginCommand(request));

                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest("Invalid credentials");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var result = await _mediator.Send(new RegisterCommand(request));

                if (result)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> RefreshJwt(RefreshTokenRequest refreshToken)
        {
            try
            {
                var result = await _mediator.Send(new RefreshCommand(refreshToken));

                if (result != null)
                {
                    return Ok(result);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("userData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserData()
        {
            try
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value.ToString();

                if (username != null)
                {
                    var result = await _mediator.Send(new GetUserDataQuery(username));

                    if (result != null)
                    {
                        return Ok(result);
                    }
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
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
