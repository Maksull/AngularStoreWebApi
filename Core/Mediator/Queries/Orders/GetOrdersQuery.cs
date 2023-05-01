using Core.Entities;
using MediatR;

namespace Core.Mediator.Queries.Orders
{
    public sealed record GetOrdersQuery() : IRequest<IEnumerable<Order>>;
}
