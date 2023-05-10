using Core.Entities;
using MediatR;
using System.Security.Claims;

namespace Core.Mediator.Queries.Orders
{
    public sealed record GetOrdersByUserIdQuery(ClaimsPrincipal User) : IRequest<IEnumerable<Order>>;
}
