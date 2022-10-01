using WebApi.Models.Database;

namespace WebApi.Models.Repository
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly ApiDataContext _context;

        public OrderRepository(ApiDataContext context)
        {
            _context = context;
        }

        public IQueryable<Order> Orders => _context.Orders;

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task SaveOrderAsync(Order order)
        {
            await _context.SaveChangesAsync();
        }
    }
}
