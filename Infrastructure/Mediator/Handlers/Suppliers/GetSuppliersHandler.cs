using Core.Entities;
using Core.Mediator.Queries.Suppliers;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class GetSuppliersHandler : IRequestHandler<GetSuppliersQuery, IEnumerable<Supplier>>
    {
        private readonly ISupplierService _supplierService;

        public GetSuppliersHandler(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public Task<IEnumerable<Supplier>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_supplierService.GetSuppliers());
        }
    }
}
