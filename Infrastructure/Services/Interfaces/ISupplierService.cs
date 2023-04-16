using Core.Entities;

namespace Infrastructure.Services.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<Supplier>? GetSuppliers();
        Task<Supplier?> GetSupplier(long id);
        Task<Supplier> CreateSupplier(Supplier supplier);
        Task<Supplier?> UpdateSupplier(Supplier supplier);
        Task<Supplier?> DeleteSupplier(long id);
    }
}
