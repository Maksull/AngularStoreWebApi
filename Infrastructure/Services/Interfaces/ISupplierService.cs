using Core.Contracts.Controllers.Suppliers;
using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetSuppliers();
        Task<Supplier?> GetSupplier(long id);
        Task<Supplier> CreateSupplier(CreateSupplierRequest createSupplier);
        Task<Supplier?> UpdateSupplier(UpdateSupplierRequest updateSupplier);
        Task<Supplier?> DeleteSupplier(long id);
    }
}
