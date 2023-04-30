using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public sealed class SupplierRepositoryTests
    {
        [Fact]
        public void Suppliers_WhenCalled_ReturnSuppliers()
        {
            Supplier[] suppliers =
            {
                new()
                {
                    SupplierId = 1,
                    Name = "First",
                    City = "CityFirst",
                },
                new()
                {
                    SupplierId = 2,
                    Name = "Second",
                    City = "CitySecond",
                }
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "Suppliers")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                SupplierRepository supplierRepository = new(context);
                var result = (supplierRepository.Suppliers.AsNoTracking()).ToArray();

                result.Should().BeOfType<Supplier[]>();
                result.Should().BeEquivalentTo(suppliers);
            }
        }

        [Fact]
        public async void CreateSupplierAsync_WhenCalled_AddSupplier()
        {
            Supplier[] suppliers =
            {
                new()
                {
                    SupplierId = 1,
                    Name = "First",
                    City = "CityFirst",
                },
                new()
                {
                    SupplierId = 2,
                    Name = "Second",
                    City = "CitySecond",
                }
            };
            Supplier supplier = new()
            {
                SupplierId = 0,
                Name = "Test",
                City = "CityTest",
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "CreateSupplier")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                SupplierRepository supplierRepository = new(context);
                var oldResult = (supplierRepository.Suppliers.AsNoTracking()).ToArray();
                await supplierRepository.CreateSupplierAsync(supplier);
                var newResult = (supplierRepository.Suppliers.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Supplier[]>();
                oldResult.Should().BeEquivalentTo(suppliers);
                newResult.Should().BeOfType<Supplier[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(suppliers.Length + 1);
            }
        }

        [Fact]
        public async void UpdateSupplierAsync_WhenCalled_UpdateSupplier()
        {
            Supplier supplier = new()
            {
                SupplierId = 2,
                Name = "Second",
                City = "CitySecond",
            };
            Supplier[] suppliers =
            {
                new()
                {
                    SupplierId = 1,
                    Name = "First",
                    City = "CityFirst",
                },
                supplier,
            };



            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateSupplier")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                SupplierRepository supplierRepository = new(context);
                supplier.Name = "UpdatedName";
                supplier.City = "CityUpdated";

                await supplierRepository.UpdateSupplierAsync(supplier);
                var result = (supplierRepository.Suppliers.AsNoTracking()).ToArray();

                result.Should().BeOfType<Supplier[]>();
                result.Should().HaveCount(suppliers.Length);
                result[1].Should().BeEquivalentTo(supplier);
            }
        }

        [Fact]
        public async void DeleteSupplierAsync_WhenCalled_DeleteSupplier()
        {
            Supplier supplier = new()
            {
                SupplierId = 2,
                Name = "Second",
                City = "CitySecond",
            };
            Supplier[] suppliers =
            {
                new()
                {
                    SupplierId = 1,
                    Name = "First",
                    City = "CityFirst",
                },
                supplier,
            };

            var options = new DbContextOptionsBuilder<ApiDataContext>()
                .UseInMemoryDatabase(databaseName: "DeleteSupplier")
                .Options;

            using (var context = new ApiDataContext(options))
            {
                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

            using (var context = new ApiDataContext(options))
            {
                SupplierRepository supplierRepository = new(context);
                var oldResult = (supplierRepository.Suppliers.AsNoTracking()).ToArray();
                await supplierRepository.DeleteSupplierAsync(supplier);
                var newResult = (supplierRepository.Suppliers.AsNoTracking()).ToArray();

                oldResult.Should().BeOfType<Supplier[]>();
                oldResult.Should().BeEquivalentTo(suppliers);
                newResult.Should().BeOfType<Supplier[]>();
                newResult.Should().NotBeNullOrEmpty();
                newResult.Should().HaveCount(suppliers.Length - 1);
            }
        }
    }
}
