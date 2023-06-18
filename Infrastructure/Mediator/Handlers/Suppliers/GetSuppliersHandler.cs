using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Suppliers;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Suppliers
{
    public sealed class GetSuppliersHandler : IRequestHandler<GetSuppliersQuery, IEnumerable<Supplier>>
    {
        private readonly ISupplierService _supplierService;
        private readonly IMetrics _metrics;

        public GetSuppliersHandler(ISupplierService supplierService, IMetrics metrics)
        {
            _supplierService = supplierService;
            _metrics = metrics;
        }

        public Task<IEnumerable<Supplier>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = _supplierService.GetSuppliers();

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetSuppliersCounter);

            return Task.FromResult(suppliers);
        }
    }
}
