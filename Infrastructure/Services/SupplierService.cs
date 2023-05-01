using Core.Contracts.Controllers.Suppliers;
using Core.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public sealed class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            IEnumerable<Supplier> suppliers = _unitOfWork.Supplier.Suppliers.Include(s => s.Products);

            foreach (var s in suppliers)
            {
                foreach (var p in s.Products!)
                {
                    p.Supplier = null;
                }
            }

            return suppliers;
        }

        public async Task<Supplier?> GetSupplier(long id)
        {
            string key = $"SupplierId={id}";

            var cachedSupplier = await _cacheService.GetAsync<Supplier>(key);

            if (cachedSupplier != null)
            {
                return cachedSupplier;
            }

            Supplier? supplier = await _unitOfWork.Supplier.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.SupplierId == id);

            if (supplier != null)
            {
                foreach (var p in supplier.Products!)
                {
                    p.Supplier = null;
                }

                await _cacheService.SetAsync(key, supplier);
            }

            return supplier;
        }

        public async Task<Supplier> CreateSupplier(CreateSupplierRequest createSupplier)
        {
            Supplier supplier = _mapper.Map<Supplier>(createSupplier);

            await _unitOfWork.Supplier.CreateSupplierAsync(supplier);

            return supplier;
        }

        public async Task<Supplier?> UpdateSupplier(UpdateSupplierRequest updateSupplier)
        {
            Supplier supplier = _mapper.Map<Supplier>(updateSupplier);

            var t = await _unitOfWork.Supplier.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.SupplierId == supplier.SupplierId);

            if (t != null)
            {
                await _unitOfWork.Supplier.UpdateSupplierAsync(supplier);

                return supplier;
            }

            return null;
        }

        public async Task<Supplier?> DeleteSupplier(long id)
        {
            Supplier? supplier = await _unitOfWork.Supplier.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.SupplierId == id);

            if (supplier != null)
            {
                string key = $"SupplierId={id}";

                await _unitOfWork.Supplier.DeleteSupplierAsync(supplier);

                await _cacheService.RemoveAsync(key);

                return supplier;
            }

            return null;
        }

    }
}
