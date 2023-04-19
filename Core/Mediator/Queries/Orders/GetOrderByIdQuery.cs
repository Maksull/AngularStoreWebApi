using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Orders
{
    public sealed record GetOrderByIdQuery(long Id) : IRequest<Order?>;
}
