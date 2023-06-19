using Core.Contracts.Controllers.Orders;
using Core.Entities;
using MediatR;
using System.Security.Claims;

namespace Core.Mediator.Commands.Orders
{
    public sealed record CreateOrderCommand(CreateOrderRequest Order, ClaimsPrincipal User) : IRequest<Order>;
}
