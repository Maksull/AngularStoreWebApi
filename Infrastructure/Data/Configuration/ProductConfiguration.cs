using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            Product[] products =
            {
                new()
                {
                    ProductId = 1,
                    Name = "Kayak",
                    Description = "A boat for one person",
                    Price = 275,
                    CategoryId = 1,
                    SupplierId = 1,
                    Images = "Kayak/kayak.png"
                },
                new()
                {
                    ProductId = 2,
                    Name = "Lifejacket",
                    Description = "Protective and fashionable",
                    Price = 48.95m,
                    CategoryId = 1,
                    SupplierId = 1,
                    Images = "Lifejacket/lifejacket.png"
                },
                new()
                {
                    ProductId = 3,
                    Name = "Ball",
                    Description = "The best size and weight",
                    Price = 19.50m,
                    CategoryId = 2,
                    SupplierId = 2,
                    Images = "Ball/ball.png"
                },
                new()
                {
                    ProductId = 4,
                    Name = "Corner Flags",
                    Description = "Give your playing field a professional touch",
                    Price = 34.95m,
                    CategoryId = 2,
                    SupplierId = 2,
                    Images = "Corner flags/corner_flags.png"
                },
                new()
                {
                    ProductId = 5,
                    Name = "Stadium",
                    Description = "Flat-packed 35,000-seat stadium",
                    Price = 79500,
                    CategoryId = 2,
                    SupplierId = 2,
                    Images = "Stadium/stadium.png"
                },
                new()
                {
                    ProductId = 6,
                    Name = "Thinking Cap",
                    Description = "Improve brain efficiency by 75%",
                    Price = 16,
                    CategoryId = 3,
                    SupplierId = 3,
                    Images = "Thinking cap/thinking_cap.png"
                },
                new()
                {
                    ProductId = 7,
                    Name = "Unsteady Chair",
                    Description = "Secretly give your opponent a disadvantage",
                    Price = 29.95m,
                    CategoryId = 3,
                    SupplierId = 3,
                    Images = "Unsteady chair/unsteady_chair.png"
                },
                new()
                {
                    ProductId = 8,
                    Name = "Human Chess Board",
                    Description = "A fun game for the family",
                    Price = 75,
                    CategoryId = 3,
                    SupplierId = 3,
                    Images = "Human chess board/human_chess_board.png"
                },
                new()
                {
                    ProductId = 9,
                    Name = "T-shirt",
                    Description = "Just t-shirt",
                    Price = 1200,
                    CategoryId = 3,
                    SupplierId = 3,
                    Images = "T-shirt/t_shirt.png"
                }
            };

            builder.HasData(products);
        }
    }
}
