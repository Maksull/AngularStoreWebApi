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
        private readonly Serilog.ILogger _logger;

        public RatingsController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of ratings.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/ratings
        ///     
        /// </remarks>
        /// <returns>Returns the list of ratings</returns>
        /// <response code="200">Returns the list of ratings</response>
        /// <response code="404">If the ratings do not exist</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Rating>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetRatings()
        {
            var ratings = await _mediator.Send(new GetRatingsQuery());

            if (ratings.Any())
            {
                _logger.Information("Ratings found. Count: {RatingsCount}.", ratings.Count());

                return Ok(ratings);
            }
            _logger.Information("No Ratings found.");

            return NotFound();
        }

        /// <summary>
        /// Gets a rating by product id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/ratings/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the product. This ID is used to retrieve a ratings for product from the database.</param>
        /// <returns>Returns the list of ratings</returns>
        /// <response code="200">Returns the list of ratings</response>
        /// <response code="404">If the ratings does not exist</response>
        [HttpGet("productId/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Rating>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetRatingsByProductId([FromRoute] long id)
        {
            var ratings = await _mediator.Send(new GetRatingsByProductIdQuery(id));

            if (ratings.Any())
            {
                _logger.Information("Ratings found. Count: {RatingsCount}.", ratings.Count());

                return Ok(ratings);
            }
            _logger.Information("No Ratings found.");

            return NotFound();
        }

        /// <summary>
        /// Gets a ratings by user's id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/ratings/userId
        ///     
        /// </remarks>
        /// <returns>Returns the list of ratings with same user's id</returns>
        /// <response code="200">Returns the list of ratings with same user's id</response>
        /// <response code="404">If the ratings does not exist</response>
        [HttpGet("userId")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Rating>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetRatingsByUserId()
        {
            var ratings = await _mediator.Send(new GetRatingsByUserIdQuery(User));

            if (ratings.Any())
            {
                _logger.Information("Ratings found. Count: {RatingsCount}.", ratings.Count());

                return Ok(ratings);
            }
            _logger.Information("No Ratings found.");

            return NotFound();
        }

        /// <summary>
        /// Gets a rating by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/ratings/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the rating. This ID is used to retrieve a specific rating from the database.</param>
        /// <returns>A rating</returns>
        /// <response code="200">Returns the rating</response>
        /// <response code="404">If the rating does not exist</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> GetRating([FromRoute] Guid id)
        {
            var rating = await _mediator.Send(new GetRatingByIdQuery(id));

            if (rating != null)
            {
                _logger.Information("Rating found. RatingId: {RatingId}.", id);

                return Ok(rating);
            }
            _logger.Information("Rating not found. RatingId: {RatingId}.", id);

            return NotFound();
        }

        /// <summary>
        /// Creates a rating.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/ratings
        ///     {        
        ///       "productId": 1,
        ///       "value": 5,
        ///       "comment": "BEST!"
        ///     }
        /// </remarks>
        /// <param name="createRating">The rating object containing the details of the rating to be created.</param>
        /// <returns>A newly created rating</returns>
        /// <response code="200">Returns the newly created rating</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        public async Task<IActionResult> CreateRating([FromBody] CreateRatingRequest createRating)
        {
            var r = await _mediator.Send(new CreateRatingCommand(createRating, User));

            _logger.Information("Rating created. RatingId: {RatingId}.", r.RatingId);

            return Ok(r);
        }

        /// <summary>
        /// Updates a rating.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/ratings
        ///     {        
        ///       "ratingId" = "guid",
        ///       "productId": 1,
        ///       "value": 1,
        ///       "comment": "WORST!"
        ///     }
        /// </remarks>
        /// <param name="updateRating">The rating object containing the details of the rating to be updated.</param>
        /// <returns>An updated rating</returns>
        /// <response code="200">Returns the updated rating</response>
        /// <response code="404">If the rating does not exist</response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> UpdateRating([FromBody] UpdateRatingRequest updateRating)
        {
            var r = await _mediator.Send(new UpdateRatingCommand(updateRating, User));

            if (r != null)
            {
                _logger.Information("Rating updated. RatingId: {RatingId}.", r.RatingId);

                return Ok(r);
            }
            _logger.Information("Rating not found. RatingId: {RatingId}.", updateRating.RatingId);

            return NotFound();
        }

        /// <summary>
        /// Deletes a rating by its id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/ratings/1
        ///     
        /// </remarks>
        /// <param name="id">The ID of the rating. This ID is used to delete a specific rating from the database.</param>
        /// <returns>An deleted rating</returns>
        /// <response code="200">Returns the deleted rating</response>
        /// <response code="404">If the rating does not exist</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Rating))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid id)
        {
            var rating = await _mediator.Send(new DeleteRatingCommand(id));

            if (rating != null)
            {
                _logger.Information("Rating deleted. RatingId: {RatingId}.", id);

                return Ok(rating);
            }
            _logger.Information("Rating not found. RatingId: {RatingId}.", id);

            return NotFound();
        }
    }
}
