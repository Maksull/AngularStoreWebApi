using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Categories
{
    public sealed record GetCategoryByIdQuery(long Id) : IRequest<Category?>;
}
