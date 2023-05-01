namespace Core.Contracts.Controllers.Suppliers
{
    public sealed record UpdateSupplierRequest(long SupplierId, string Name, string City);
}
