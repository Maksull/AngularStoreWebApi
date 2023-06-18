using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Suppliers;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, Supplier>
    {
        private readonly ISupplierService _supplierService;
        private readonly IMetrics _metrics;

        public CreateSupplierHandler(ISupplierService supplierService, IMetrics metrics)
        {
            _supplierService = supplierService;
            _metrics = metrics;
        }

        public async Task<Supplier> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierService.CreateSupplier(request.Supplier);

            _metrics.Measure.Counter.Increment(MetricsRegistry.CreateSupplierCounter);

            return supplier;
        }
    }
}
