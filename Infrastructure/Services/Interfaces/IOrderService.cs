using Core.Contracts.Controllers.Orders;
using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
        Task<Order?> GetOrder(long id);
        Task<Order> CreateOrder(CreateOrderRequest order);
        Task<Order?> UpdateOrder(UpdateOrderRequest order);
        Task<Order?> DeleteOrder(long id);
    }
}
