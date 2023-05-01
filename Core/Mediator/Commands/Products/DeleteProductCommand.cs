using Core.Entities;
using MediatR;

namespace Core.Mediator.Commands.Products
{
    public sealed record DeleteProductCommand(long Id) : IRequest<Product?>;
}
