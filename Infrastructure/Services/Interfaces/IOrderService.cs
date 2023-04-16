using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order>? GetOrders();
        Task<Order?> GetOrder(long id);
        Task<Order> CreateOrder(Order order);
        Task<Order?> UpdateOrder(Order order);
        Task<Order?> DeleteOrder(long id);
    }
}
