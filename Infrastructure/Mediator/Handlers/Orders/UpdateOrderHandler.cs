using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Orders;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order?>
    {
        private readonly IOrderService _orderService;
        private readonly IMetrics _metrics;

        public UpdateOrderHandler(IOrderService orderService, IMetrics metrics)
        {
            _orderService = orderService;
            _metrics = metrics;
        }

        public async Task<Order?> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.UpdateOrder(request.Order);

            _metrics.Measure.Counter.Increment(MetricsRegistry.UpdateOrderCounter);

            return order;
        }
    }
}
