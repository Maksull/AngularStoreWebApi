using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, Supplier>
    {
        private readonly ISupplierService _supplierService;

        public CreateSupplierHandler(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<Supplier> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            return await _supplierService.CreateSupplier(request.Supplier);
        }
    }
}
