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
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetRatings()
        {
            try
            {
                var ratings = await _mediator.Send(new GetRatingsQuery());

                if (ratings.Any())
                {
                    return Ok(ratings);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("productId/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Rating>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetRatingsByProductId(long id)
        {
            try
            {
                var ratings = await _mediator.Send(new GetRatingsByProductIdQuery(id));

                if (ratings.Any())
                {
                    return Ok(ratings);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetRating(Guid id)
        {
            try
            {
                var rating = await _mediator.Send(new GetRatingByIdQuery(id));

                if (rating != null)
                {
                    return Ok(rating);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CreateRating(CreateRatingRequest createRating)
        {
            try
            {
                var r = await _mediator.Send(new CreateRatingCommand(createRating, User));

                return Ok(r);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateRating(UpdateRatingRequest updateRating)
        {
            try
            {
                var r = await _mediator.Send(new UpdateRatingCommand(updateRating, User));

                if (r != null)
                {
                    return Ok(r);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.InnerException!.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteRating(Guid id)
        {
            try
            {
                var rating = await _mediator.Send(new DeleteRatingCommand(id));

                if (rating != null)
                {
                    return Ok(rating);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
