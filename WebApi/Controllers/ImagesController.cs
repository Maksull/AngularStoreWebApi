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

        /// <summary>
        /// Creates an image in s3 bucket.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/images/upload
        ///     
        /// </remarks>
        /// <param name="file">The file object containing the details of the image to be created.</param>
        /// <returns>An OkObjectResult with FileName</returns>
        /// <response code="200">Returns the OkObjectResult with FileName</response>
        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var f = await _mediator.Send(new UploadImageCommand(file));

            if (f != null)
            {
                return Ok($"{f.FileName} was uploaded successfully");
            }

            return NotFound();
        }

        /// <summary>
        /// Gets an image by its key.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/images/request?key=product/product.png
        ///     
        /// </remarks>
        /// <param name="key">The key of the image store in s3 bucket. This key is used to retrieve a specific image from the s3 bucket.</param>
        /// <returns>An image</returns>
        /// <response code="200">Returns the image</response>
        /// <response code="404">If the image does not exist</response>
        [HttpGet("request")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetFile(string key)
        {
            var file = await _mediator.Send(new GetImageQuery(key));

            if (file != null)
            {
                return File(file.ResponseStream, file.Headers.ContentType);
            }

            return NotFound();
        }
    }
}
