using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.Database
{
    public sealed class ApiDataContext : DbContext
    {
        public ApiDataContext(DbContextOptions<ApiDataContext> opts): base(opts) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Order> Orders => Set<Order>();
    }
}
