using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Orders;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Order?>
    {
        private readonly IOrderService _orderService;
        private readonly IMetrics _metrics;

        public DeleteOrderHandler(IOrderService orderService, IMetrics metrics)
        {
            _orderService = orderService;
            _metrics = metrics;
        }

        public async Task<Order?> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.DeleteOrder(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.DeleteOrderCounter);

            return order;
        }
    }
}
