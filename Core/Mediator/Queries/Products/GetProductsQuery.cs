using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Products
{
    public sealed record GetProductsQuery() : IRequest<IEnumerable<Product>>;
}
