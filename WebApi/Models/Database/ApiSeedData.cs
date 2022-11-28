using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.Database
{
    public sealed class ApiSeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApiDataContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApiDataContext>();
            context.Database.Migrate();

            if (context.Products.Count() == 0 && context.Categories.Count() == 0 && context.Suppliers.Count() == 0)
            {
                Supplier s1 = new Supplier { Name = "Splash", City = "Seattle" };
                Supplier s2 = new Supplier { Name = "Your football", City = "Detroit" };
                Supplier s3 = new Supplier { Name = "My Chess", City = "New York" };

                Category c1 = new Category { Name = "Watersports" };
                Category c2 = new Category { Name = "Football" };
                Category c3 = new Category { Name = "Chess" };

                context.Products.AddRange(
                    new Product
                    {
                        Name = "Kayak",
                        Description = "A boat for one person",
                        Price = 275,
                        Category = c1,
                        Supplier = s1,
                        Images = "Kayak/kayak.png"
                    },
                    new Product
                    {
                        Name = "Lifejacket",
                        Description = "Protective and fashionable",
                        Price = 48.95m,
                        Category = c1,
                        Supplier = s1,
                        Images = "Lifejacket/lifejacket.png"
                    },
                    new Product
                    {
                        Name = "Ball",
                        Description = "The best size and weight",
                        Price = 19.50m,
                        Category = c2,
                        Supplier = s2,
                        Images = "Ball/ball.png"
                    },
                    new Product
                    {
                        Name = "Corner Flags",
                        Description = "Give your playing field a professional touch",
                        Price = 34.95m,
                        Category = c2,
                        Supplier = s2,
                        Images = "Corner flags/corner_flags.png"
                    },
                    new Product
                    {
                        Name = "Stadium",
                        Description = "Flat-packed 35,000-seat stadium",
                        Price = 79500,
                        Category = c2,
                        Supplier = s2,
                        Images = "Stadium/stadium.png"
                    },
                    new Product
                    {
                        Name = "Thinking Cap",
                        Description = "Improve brain efficiency by 75%",
                        Price = 16,
                        Category = c3,
                        Supplier = s3,
                        Images = "Thinking cap/thinking_cap.png"
                    },
                    new Product
                    {
                        Name = "Unsteady Chair",
                        Description = "Secretly give your opponent a disadvantage",
                        Price = 29.95m,
                        Category = c3,
                        Supplier = s3,
                        Images = "Unsteady chair/unsteady_chair.png"
                    },
                    new Product
                    {
                        Name = "Human Chess Board",
                        Description = "A fun game for the family",
                        Price = 75,
                        Category = c3,
                        Supplier = s3,
                        Images = "Human chess board/human_chess_board.png"
                    },
                    new Product
                    {
                        Name = "T-shirt",
                        Description = "Just t-shirt",
                        Price = 1200,
                        Category = c3,
                        Supplier = s3,
                        Images = "T-shirt/t_shirt.png"
                    }
                );
            }
            context.SaveChanges();
        }
    }
}
