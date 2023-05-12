using Core.Mediator.Commands.Images;
using Core.Mediator.Queries.Images;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public sealed class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var f = await _mediator.Send(new UploadFileCommand(file));

            if (f != null)
            {
                return Ok($"{f.FileName} was uploaded successfully");
            }

            return NotFound();
        }

        [HttpGet("request")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetFile(string key)
        {
            var file = await _mediator.Send(new GetFileQuery(key));

            if (file != null)
            {
                return File(file.ResponseStream, file.Headers.ContentType);
            }

            return NotFound();
        }
    }
}
