using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Suppliers;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, Supplier?>
    {
        private readonly ISupplierService _supplierService;
        private readonly IMetrics _metrics;

        public GetSupplierByIdHandler(ISupplierService supplierService, IMetrics metrics)
        {
            _supplierService = supplierService;
            _metrics = metrics;
        }

        public async Task<Supplier?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierService.GetSupplier(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetSupplierByIdCounter);

            return supplier;
        }
    }
}
