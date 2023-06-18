using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class DeleteSupplierHandler : IRequestHandler<DeleteSupplierCommand, Supplier?>
    {
        private readonly ISupplierService _supplierService;
        private readonly IMetrics _metrics;

        public DeleteSupplierHandler(ISupplierService supplierService, IMetrics metrics)
        {
            _supplierService = supplierService;
            _metrics = metrics;
        }

        public async Task<Supplier?> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierService.DeleteSupplier(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.DeleteSupplierCounter);

            return supplier;
        }
    }
}
