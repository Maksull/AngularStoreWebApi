using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, Supplier?>
    {
        private readonly ISupplierService _supplierService;
        private readonly IMetrics _metrics;

        public UpdateSupplierHandler(ISupplierService supplierService, IMetrics metrics)
        {
            _supplierService = supplierService;
            _metrics = metrics;
        }

        public async Task<Supplier?> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierService.UpdateSupplier(request.Supplier);

            _metrics.Measure.Counter.Increment(MetricsRegistry.UpdateSupplierCounter);

            return supplier;
        }
    }
}
