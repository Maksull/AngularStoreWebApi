using Core.Contracts.Controllers.Orders;
using Core.Entities;
using MediatR;

namespace Core.Mediator.Commands.Orders
{
    public sealed record CreateOrderCommand(CreateOrderRequest Order) : IRequest<Order>;
}
