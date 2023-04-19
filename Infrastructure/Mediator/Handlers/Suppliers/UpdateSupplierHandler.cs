using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, Supplier?>
    {
        private readonly ISupplierService _supplierService;

        public UpdateSupplierHandler(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<Supplier?> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            return await _supplierService.UpdateSupplier(request.Supplier);
        }
    }
}
