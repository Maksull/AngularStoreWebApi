namespace WebApi.Models.Repository
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }

        Task SaveOrderAsync(Order order);
        Task CreateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
    }
}
