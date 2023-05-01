using Core.Entities;
using MediatR;

namespace Core.Mediator.Commands.Orders
{
    public sealed record DeleteOrderCommand(long Id) : IRequest<Order?>;
}
