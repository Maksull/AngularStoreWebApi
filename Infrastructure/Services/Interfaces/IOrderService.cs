using Core.Contracts.Controllers.Orders;
using Core.Entities;
using System.Security.Claims;

namespace Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
        IEnumerable<Order> GetOrdersByUserId(ClaimsPrincipal user);
        Task<Order?> GetOrder(long id);
        Task<Order> CreateOrder(CreateOrderRequest order, ClaimsPrincipal user);
        Task<Order?> UpdateOrder(UpdateOrderRequest order);
        Task<Order?> DeleteOrder(long id);
    }
}
