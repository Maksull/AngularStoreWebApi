using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Orders;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order?>
    {
        private readonly IOrderService _orderService;
        private readonly IMetrics _metrics;

        public GetOrderByIdHandler(IOrderService orderService, IMetrics metrics)
        {
            _orderService = orderService;
            _metrics = metrics;
        }

        public async Task<Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrder(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetOrderByIdCounter);

            return order;
        }
    }
}
