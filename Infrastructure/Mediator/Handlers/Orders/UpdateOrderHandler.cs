using Core.Entities;
using Core.Mediator.Commands.Orders;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order?>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Order?> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            return await _orderService.UpdateOrder(request.Order);
        }
    }
}
