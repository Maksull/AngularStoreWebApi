using Core.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }

        Task CreateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
    }
}
