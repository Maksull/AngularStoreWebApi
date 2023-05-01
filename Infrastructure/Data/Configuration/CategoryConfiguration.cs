using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            Category[] categories = {
                new()
                {
                    CategoryId = 1,
                    Name = "Watersports",
                },
                new()
                {
                    CategoryId = 2,
                    Name = "Football",
                },
                new()
                {
                    CategoryId = 3,
                    Name = "Chess",
                }
            };

            builder.HasData(categories);
        }
    }
}
