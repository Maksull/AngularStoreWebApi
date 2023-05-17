using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Core.Mediator.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public AuthController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Verifies credentials and returns jwt.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/auth/login
        ///     {        
        ///       "username": "Username",
        ///       "password": "Asdsd!23$",
        ///     }
        /// </remarks>
        /// <param name="request">The login object containing the details of the user to be verified and login.</param>
        /// <returns>A jwt</returns>
        /// <response code="200">Returns the jwt</response>
        /// <response code="400">If the user's credentials were invalid</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new LoginCommand(request));

            if (result != null)
            {
                _logger.Information("Login successful for username: {Username}", request.Username);

                return Ok(result);
            }
            _logger.Information("Invalid credentials for username: {Username}", request.Username);

            return BadRequest("Invalid credentials");
        }

        /// <summary>
        /// Creates an user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/auth/register
        ///     {        
        ///       "firstName": "FirstName",
        ///       "lastName": "LastName",
        ///       "username": "Username",
        ///       "email": "your_email@col.co",
        ///       "password": "Asdsd!23$",
        ///       "confirmPassword": "Asdsd!23$"
        ///     }
        /// </remarks>
        /// <param name="request">The register object containing the details of the user to be created.</param>
        /// <returns>Returns the OkResult</returns>
        /// <response code="200">Returns the OkResult</response>
        /// <response code="400">If the user was not created</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _mediator.Send(new RegisterCommand(request));

            if (result)
            {
                _logger.Information("User registered successfully. Username: {Username}", request.Username);

                return Ok();
            }
            _logger.Information("Failed to register user. Username: {Username}", request.Username);

            return BadRequest();
        }

        /// <summary>
        /// Refresh jwt.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/auth/refresh
        ///     {        
        ///       "token": "string",
        ///       "expired": "2023-05-14T13:16:32.493Z"
        ///     }
        /// </remarks>
        /// <param name="refreshToken">The refreshToken object containing the details of the data to refresh jwt.</param>
        /// <returns>Returns a newly created jwt</returns>
        /// <response code="200">Returns the newly created jwt</response>
        /// <response code="401">If the refresh token was invalid</response>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        public async Task<IActionResult> RefreshJwt([FromBody] RefreshTokenRequest refreshToken)
        {
            var result = await _mediator.Send(new RefreshCommand(refreshToken));

            if (result != null)
            {
                _logger.Information("JWT refreshed successfully.");

                return Ok(result);
            }
            _logger.Information("Invalid refresh token");

            return Unauthorized();
        }

        /// <summary>
        /// Gets an user's data by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/auth/userData
        ///     
        /// </remarks>
        /// <returns>Returns the user data of user by its id</returns>
        /// <response code="200">Returns the user data of user by its id</response>
        /// <response code="404">If the user does not exist</response>
        [Authorize]
        [HttpGet("userData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetUserData()
        {
            var result = await _mediator.Send(new GetUserDataQuery(User));

            if (result != null)
            {
                _logger.Information("User data retrieved successfully");

                return Ok(result);
            }
            _logger.Information("User not found");

            return NotFound();
        }

        /// <summary>
        /// Check if user is authenticated.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/auth/protected
        ///     
        /// </remarks>
        /// <returns>Returns OkResult if user is authenticated</returns>
        /// <response code="200">If the user is authenticated</response>
        [Authorize]
        [HttpGet("protected")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
        public IActionResult Protected()
        {
            _logger.Information("Protected method called.");

            return Ok();
        }

        /// <summary>
        /// Check if user is authenticated as admin.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/auth/adminProtected
        ///     
        /// </remarks>
        /// <returns>Returns OkResult if user is authenticated as admin</returns>
        /// <response code="200">If the user is authenticated as admin</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("adminProtected")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
        public IActionResult AdminProtected()
        {
            _logger.Information("AdminProtected method called.");

            return Ok();
        }
    }
}
