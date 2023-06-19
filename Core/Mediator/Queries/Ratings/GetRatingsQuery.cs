using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Ratings
{
    public sealed record GetRatingsQuery() : IRequest<IEnumerable<Rating>>;
}
