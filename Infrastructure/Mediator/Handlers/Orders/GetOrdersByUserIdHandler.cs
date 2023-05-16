using Core.Entities;
using Core.Mediator.Queries.Orders;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Orders
{
    public sealed class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserIdQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByUserIdHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<IEnumerable<Order>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_orderService.GetOrdersByUserId(request.User));
        }
    }
}
