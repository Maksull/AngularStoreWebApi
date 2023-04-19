using Core.Entities;
using Core.Mediator.Queries.Suppliers;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, Supplier?>
    {
        private readonly ISupplierService _supplierService;

        public GetSupplierByIdHandler(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<Supplier?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            return await _supplierService.GetSupplier(request.Id);
        }
    }
}
