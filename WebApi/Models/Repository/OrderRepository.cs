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
            foreach (var l in order.Lines!)
            {
                l.Product!.Category = null;
                l.Product!.Supplier = null;
            }
            _context.AttachRange(order.Lines!.Select(l => l.Product!));
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
