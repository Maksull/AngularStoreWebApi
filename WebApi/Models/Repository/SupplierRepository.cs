using WebApi.Models.Database;

namespace WebApi.Models.Repository
{
    public sealed class SupplierRepository : ISupplierRepository
    {
        private readonly ApiDataContext _context;

        public SupplierRepository(ApiDataContext context)
        {
            _context = context;
        }

        public IQueryable<Supplier> Suppliers => _context.Suppliers;

        public async Task CreateSupplierAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
