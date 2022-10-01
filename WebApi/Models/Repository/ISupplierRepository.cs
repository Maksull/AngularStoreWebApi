namespace WebApi.Models.Repository
{
    public interface ISupplierRepository
    {
        IQueryable<Supplier> Suppliers { get; }

        Task SaveSupplierAsync(Supplier supplier);
        Task CreateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(Supplier supplier);
    }
}
