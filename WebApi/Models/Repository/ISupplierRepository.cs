namespace WebApi.Models.Repository
{
    public interface ISupplierRepository
    {
        IQueryable<Supplier> Suppliers { get; }

        Task CreateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
    }
}
