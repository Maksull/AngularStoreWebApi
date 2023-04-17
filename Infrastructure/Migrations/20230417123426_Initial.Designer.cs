﻿// <auto-generated />
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApiDataContext))]
    [Migration("20230417123426_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.CartLine", b =>
                {
                    b.Property<long>("CartLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CartLineId"));

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("CartLineId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartLine");
                });

            modelBuilder.Entity("Core.Entities.Category", b =>
                {
                    b.Property<long>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1L,
                            Name = "Watersports"
                        },
                        new
                        {
                            CategoryId = 2L,
                            Name = "Football"
                        },
                        new
                        {
                            CategoryId = 3L,
                            Name = "Chess"
                        });
                });

            modelBuilder.Entity("Core.Entities.Order", b =>
                {
                    b.Property<long>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("OrderId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsShipped")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Core.Entities.Product", b =>
                {
                    b.Property<long>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ProductId"));

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Images")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("SupplierId")
                        .HasColumnType("bigint");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1L,
                            CategoryId = 1L,
                            Description = "A boat for one person",
                            Images = "Kayak/kayak.png",
                            Name = "Kayak",
                            Price = 275m,
                            SupplierId = 1L
                        },
                        new
                        {
                            ProductId = 2L,
                            CategoryId = 1L,
                            Description = "Protective and fashionable",
                            Images = "Lifejacket/lifejacket.png",
                            Name = "Lifejacket",
                            Price = 48.95m,
                            SupplierId = 1L
                        },
                        new
                        {
                            ProductId = 3L,
                            CategoryId = 2L,
                            Description = "The best size and weight",
                            Images = "Ball/ball.png",
                            Name = "Ball",
                            Price = 19.50m,
                            SupplierId = 2L
                        },
                        new
                        {
                            ProductId = 4L,
                            CategoryId = 2L,
                            Description = "Give your playing field a professional touch",
                            Images = "Corner flags/corner_flags.png",
                            Name = "Corner Flags",
                            Price = 34.95m,
                            SupplierId = 2L
                        },
                        new
                        {
                            ProductId = 5L,
                            CategoryId = 2L,
                            Description = "Flat-packed 35,000-seat stadium",
                            Images = "Stadium/stadium.png",
                            Name = "Stadium",
                            Price = 79500m,
                            SupplierId = 2L
                        },
                        new
                        {
                            ProductId = 6L,
                            CategoryId = 3L,
                            Description = "Improve brain efficiency by 75%",
                            Images = "Thinking cap/thinking_cap.png",
                            Name = "Thinking Cap",
                            Price = 16m,
                            SupplierId = 3L
                        },
                        new
                        {
                            ProductId = 7L,
                            CategoryId = 3L,
                            Description = "Secretly give your opponent a disadvantage",
                            Images = "Unsteady chair/unsteady_chair.png",
                            Name = "Unsteady Chair",
                            Price = 29.95m,
                            SupplierId = 3L
                        },
                        new
                        {
                            ProductId = 8L,
                            CategoryId = 3L,
                            Description = "A fun game for the family",
                            Images = "Human chess board/human_chess_board.png",
                            Name = "Human Chess Board",
                            Price = 75m,
                            SupplierId = 3L
                        },
                        new
                        {
                            ProductId = 9L,
                            CategoryId = 3L,
                            Description = "Just t-shirt",
                            Images = "T-shirt/t_shirt.png",
                            Name = "T-shirt",
                            Price = 1200m,
                            SupplierId = 3L
                        });
                });

            modelBuilder.Entity("Core.Entities.Supplier", b =>
                {
                    b.Property<long>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SupplierId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SupplierId");

                    b.ToTable("Suppliers");

                    b.HasData(
                        new
                        {
                            SupplierId = 1L,
                            City = "Seattle",
                            Name = "Splash"
                        },
                        new
                        {
                            SupplierId = 2L,
                            City = "Detroit",
                            Name = "Your football"
                        },
                        new
                        {
                            SupplierId = 3L,
                            City = "New York",
                            Name = "My Chess"
                        });
                });

            modelBuilder.Entity("Core.Entities.CartLine", b =>
                {
                    b.HasOne("Core.Entities.Order", null)
                        .WithMany("Lines")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Core.Entities.Product", b =>
                {
                    b.HasOne("Core.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Core.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Core.Entities.Order", b =>
                {
                    b.Navigation("Lines");
                });

            modelBuilder.Entity("Core.Entities.Supplier", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}