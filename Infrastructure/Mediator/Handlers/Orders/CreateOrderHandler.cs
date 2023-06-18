using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Orders;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IOrderService _orderService;
        private readonly IMetrics _metrics;

        public CreateOrderHandler(IOrderService orderService, IMetrics metrics)
        {
            _orderService = orderService;
            _metrics = metrics;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.CreateOrder(request.Order, request.User);

            _metrics.Measure.Counter.Increment(MetricsRegistry.CreateOrderCounter);

            return order;
        }
    }
}
