using Core.Entities;
using Core.Mediator.Commands.Orders;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Order?>
    {
        private readonly IOrderService _orderService;

        public DeleteOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Order?> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            return await _orderService.DeleteOrder(request.Id);
        }
    }
}
