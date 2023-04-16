using Core.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public sealed class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Supplier>? GetSuppliers()
        {
            if (_unitOfWork.Supplier.Suppliers != null)
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

            return null;
        }

        public async Task<Supplier?> GetSupplier(long id)
        {
            if (_unitOfWork.Supplier.Suppliers != null)
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

        public async Task<Supplier> CreateSupplier(Supplier supplier)
        {
            await _unitOfWork.Supplier.CreateSupplierAsync(supplier);

            return supplier;
        }

        public async Task<Supplier?> UpdateSupplier(Supplier supplier)
        {
            if (await _unitOfWork.Supplier.Suppliers.ContainsAsync(supplier))
            {
                await _unitOfWork.Supplier.UpdateSupplierAsync(supplier);

                return supplier;
            }

            return null;
        }

        public async Task<Supplier?> DeleteSupplier(long id)
        {
            if (_unitOfWork.Supplier.Suppliers != null)
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
