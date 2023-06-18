using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Orders;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;
        private readonly IMetrics _metrics;

        public GetOrdersHandler(IOrderService orderService, IMetrics metrics)
        {
            _orderService = orderService;
            _metrics = metrics;
        }

        public Task<IEnumerable<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = _orderService.GetOrders();

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetOrdersCounter);

            return Task.FromResult(orders);
        }
    }
}
