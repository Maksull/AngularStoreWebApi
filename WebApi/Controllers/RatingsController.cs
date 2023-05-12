using Core.Contracts.Controllers.Ratings;
using Core.Entities;
using Core.Mediator.Commands.Ratings;
using Core.Mediator.Queries.Ratings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public sealed class RatingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Rating>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetRatings()
        {
            var ratings = await _mediator.Send(new GetRatingsQuery());

            if (ratings.Any())
            {
                return Ok(ratings);
            }

            return NotFound();
        }

        [HttpGet("productId/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Rating>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetRatingsByProductId(long id)
        {
            var ratings = await _mediator.Send(new GetRatingsByProductIdQuery(id));

            if (ratings.Any())
            {
                return Ok(ratings);
            }

            return NotFound();
        }

        [HttpGet("userId")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Rating>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetRatingsByUserId()
        {
            var ratings = await _mediator.Send(new GetRatingsByUserIdQuery(User));

            if (ratings.Any())
            {
                return Ok(ratings);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetRating(Guid id)
        {
            var rating = await _mediator.Send(new GetRatingByIdQuery(id));

            if (rating != null)
            {
                return Ok(rating);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        public async Task<IActionResult> CreateRating(CreateRatingRequest createRating)
        {
            var r = await _mediator.Send(new CreateRatingCommand(createRating, User));

            return Ok(r);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateRating(UpdateRatingRequest updateRating)
        {
            var r = await _mediator.Send(new UpdateRatingCommand(updateRating, User));

            if (r != null)
            {
                return Ok(r);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteRating(Guid id)
        {
            var rating = await _mediator.Send(new DeleteRatingCommand(id));

            if (rating != null)
            {
                return Ok(rating);
            }

            return NotFound();
        }
    }
}
