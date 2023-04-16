using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            Supplier[] suppliers =
            {
                new()
                {
                    SupplierId = 1,
                    Name = "Splash",
                    City = "Seattle",
                },
                new()
                {
                    SupplierId = 2,
                    Name = "Your football",
                    City = "Detroit",
                },
                new()
                {
                    SupplierId = 3,
                    Name = "My Chess",
                    City = "New York",
                }
            };

            builder.HasData(suppliers);
        }
    }
}
