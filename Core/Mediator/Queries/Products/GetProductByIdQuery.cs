using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Products
{
    public sealed record GetProductByIdQuery(long Id) : IRequest<Product?>;
}
