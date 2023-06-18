using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Products;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Products
{
    public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductService _productService;
        private readonly IMetrics _metrics;

        public GetProductsHandler(IProductService productService, IMetrics metrics)
        {
            _productService = productService;
            _metrics = metrics;
        }

        public Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = _productService.GetProducts();

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetProductsCounter);

            return Task.FromResult(products);
        }
    }
}
