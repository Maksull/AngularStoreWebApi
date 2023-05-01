using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class DeleteSupplierHandler : IRequestHandler<DeleteSupplierCommand, Supplier?>
    {
        private readonly ISupplierService _supplierService;

        public DeleteSupplierHandler(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<Supplier?> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            return await _supplierService.DeleteSupplier(request.Id);
        }
    }
}
