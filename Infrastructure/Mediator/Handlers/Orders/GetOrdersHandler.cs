using Core.Entities;
using Core.Mediator.Queries.Orders;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<IEnumerable<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_orderService.GetOrders());
        }
    }
}
