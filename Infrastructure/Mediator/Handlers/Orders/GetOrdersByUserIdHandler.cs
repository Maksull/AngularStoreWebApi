using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Orders;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserIdQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;
        private readonly IMetrics _metrics;

        public GetOrdersByUserIdHandler(IOrderService orderService, IMetrics metrics)
        {
            _orderService = orderService;
            _metrics = metrics;
        }

        public Task<IEnumerable<Order>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = _orderService.GetOrdersByUserId(request.User);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetOrderByUserIdCounter);

            return Task.FromResult(orders);
        }
    }
}
