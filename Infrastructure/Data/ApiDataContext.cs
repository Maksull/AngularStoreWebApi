using Core.Entities;
using Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public sealed class ApiDataContext : DbContext
    {
        public ApiDataContext(DbContextOptions<ApiDataContext> opts) : base(opts) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new SupplierConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Order> Orders => Set<Order>();
    }
}
