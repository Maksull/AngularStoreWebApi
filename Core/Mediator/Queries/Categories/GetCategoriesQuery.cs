using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Categories
{
    public sealed record GetCategoriesQuery() : IRequest<IEnumerable<Category>>;
}
