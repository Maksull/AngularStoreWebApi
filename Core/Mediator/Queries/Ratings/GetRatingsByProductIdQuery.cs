using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Ratings
{
    public sealed record GetRatingsByProductIdQuery(long Id) : IRequest<IEnumerable<Rating>>;
}
