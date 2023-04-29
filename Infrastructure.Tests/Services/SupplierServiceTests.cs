using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using Infrastructure.Mapster;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using Mapster;
using MapsterMapper;
using MockQueryable.Moq;
using Moq;

namespace Infrastructure.Tests.Services
{
    public sealed class SupplierServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<ICacheService> _cacheService;
        private readonly SupplierService _supplierService;

        public SupplierServiceTests()
        {
            _unitOfWork = new();
            _mapper = GetMapper();
            _cacheService = new();
            _supplierService = new(_unitOfWork.Object, _mapper, _cacheService.Object); ;
        }


        #region GetSuppliers

        [Fact]
        public void GetSuppliers_WhenCalled_ReturnSuppliers()
        {
            //Arrange
            Supplier[] suppliers = new Supplier[]
            {
                new()
                {
                    SupplierId = 1, Name = "First", City = "CityFirst", Products = new List<Product>()
                },
                new()
                {
                    SupplierId = 2, Name = "Second", City = "CityFirst", Products = new List<Product>()
                },
                new()
                {
                    SupplierId = 3, Name = "Third", City = "CityFirst", Products = new List<Product>()
                },
            };

            _unitOfWork.Setup(u => u.Supplier.Suppliers).Returns(suppliers.AsQueryable());

            //Act
            var result = _supplierService.GetSuppliers().ToArray();


            //Arrange
            result.Should().BeOfType<Supplier[]>();
            result.Should().BeEquivalentTo(suppliers);
        }

        #endregion


        #region GetSupplier

        [Fact]
        public void GetSupplier_WhenCache_ReturnSupplier()
        {
            //Arrange
            Supplier supplier = new()
            {
                SupplierId = 1,
                Name = "First",
                City = "CityFirst",
                Products = new List<Product>()
            };
            _cacheService.Setup(u => u.GetAsync<Supplier>(It.IsAny<string>())).ReturnsAsync(supplier);

            //Act
            var result = _supplierService.GetSupplier(1).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
            result.Should().BeEquivalentTo(supplier);
        }

        [Fact]
        public void GetSupplier_WhenNoCache_ReturnSupplier()
        {
            //Arrange
            Supplier[] suppliers = new Supplier[]
            {
                new()
                {
                    SupplierId = 1, Name = "First", City = "CityFirst", Products = new List<Product>()
                },
                new()
                {
                    SupplierId = 2, Name = "Second", City = "CityFirst", Products = new List<Product>()
                },
                new()
                {
                    SupplierId = 3, Name = "Third", City = "CityFirst", Products = new List<Product>()
                },
            };
            var mock = suppliers.AsQueryable().BuildMock();

            _cacheService.Setup(u => u.GetAsync<Supplier>(It.IsAny<string>())).ReturnsAsync((Supplier)null!);
            _unitOfWork.Setup(u => u.Supplier.Suppliers).Returns(mock);

            //Act
            var result = _supplierService.GetSupplier(1).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
            result.Should().BeEquivalentTo(suppliers[0]);
        }

        #endregion


        #region CreateSupplier

        [Fact]
        public void CreateSupplier_WhenCalled_ReturnSupplier()
        {
            //Arrange
            CreateSupplierRequest createSupplier = new("First", "CityFirst");
            Supplier supplier = new()
            {
                SupplierId = 0,
                Name = "First",
                City = "CityFirst",
                Products = null
            };
            _unitOfWork.Setup(u => u.Supplier.CreateSupplierAsync(It.IsAny<Supplier>())).Returns(Task.CompletedTask);

            //Act
            var result = _supplierService.CreateSupplier(createSupplier).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
            result.Should().BeEquivalentTo(supplier);
        }

        #endregion


        #region UpdateSupplier

        [Fact]
        public void UpdateSupplier_WhenCalled_ReturnSupplier()
        {
            //Arrange
            UpdateSupplierRequest updateSupplier = new(1, "First", "TestCity");

            Supplier[] suppliers = { new() { SupplierId = updateSupplier.SupplierId, Name = updateSupplier.Name, City = updateSupplier.City } };

            var mock = suppliers.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Supplier.Suppliers).Returns(mock);

            //Act
            var result = _supplierService.UpdateSupplier(updateSupplier).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
        }

        [Fact]
        public void UpdateSupplier_WhenCalled_ReturnNull()
        {
            //Arrange
            UpdateSupplierRequest updateSupplier = new(1, "First", "CityFirst");
            Supplier[] suppliers = Array.Empty<Supplier>();
            var mock = suppliers.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Supplier.Suppliers).Returns(mock);

            //Act
            var result = _supplierService.UpdateSupplier(updateSupplier).Result;

            //Assert
            result.Should().BeNull();
        }

        #endregion


        #region DeleteSupplier

        [Fact]
        public void DeleteSupplier_WhenCalled_ReturnSupplier()
        {
            //Arrange
            Supplier[] suppliers = new Supplier[]
            {
                new()
                {
                    SupplierId = 1, Name = "First", City = "CityFirst", Products = new List<Product>()
                },
                new()
                {
                    SupplierId = 2, Name = "Second", City = "CityFirst", Products = new List<Product>()
                },
                new()
                {
                    SupplierId = 3, Name = "Third", City = "CityFirst", Products = new List<Product>()
                },
            };

            var mock = suppliers.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Supplier.Suppliers).Returns(mock);

            //Act
            var result = _supplierService.DeleteSupplier(1).Result;

            //Assert
            result.Should().BeOfType<Supplier>();
        }

        [Fact]
        public void DeleteSupplier_WhenCalled_ReturnNull()
        {
            //Arrange
            Supplier[] suppliers = Array.Empty<Supplier>();
            var mock = suppliers.AsQueryable().BuildMock();

            _unitOfWork.Setup(u => u.Supplier.Suppliers).Returns(mock);

            //Act
            var result = _supplierService.DeleteSupplier(1).Result;

            //Assert
            result.Should().BeNull();
        }

        #endregion


        private static Mapper GetMapper()
        {
            TypeAdapterConfig config = new();
            config.Apply(new MapsterRegister());

            return new Mapper(config);
        }
    }
}
