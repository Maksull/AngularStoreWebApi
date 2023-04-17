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

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            if (_unitOfWork.Supplier.Suppliers.Any())
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

            return Enumerable.Empty<Supplier>();
        }

        public async Task<Supplier?> GetSupplier(long id)
        {
            if (_unitOfWork.Supplier.Suppliers.Any())
            {
                Supplier? supplier = await _unitOfWork.Supplier.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.SupplierId == id);

                if (supplier != null)
                {
                    foreach (var p in supplier.Products!)
                    {
                        p.Supplier = null;
                    }

                    return supplier;
                }
            }

            return null;
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

            if (await _unitOfWork.Supplier.Suppliers.ContainsAsync(supplier))
            {
                await _unitOfWork.Supplier.UpdateSupplierAsync(supplier);

                return supplier;
            }

            return null;
        }

        public async Task<Supplier?> DeleteSupplier(long id)
        {
            if (_unitOfWork.Supplier.Suppliers.Any())
            {
                Supplier? supplier = await _unitOfWork.Supplier.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id);

                if (supplier != null)
                {
                    await _unitOfWork.Supplier.DeleteSupplierAsync(supplier);

                    return supplier;
                }
            }

            return null;
        }

    }
}
