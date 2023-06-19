using App.Metrics;
using Core.Entities;
using Core.Mediator.Queries.Products;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Products
{
    public sealed class GetProductsByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductService _productService;
        private readonly IMetrics _metrics;

        public GetProductsByIdHandler(IProductService productService, IMetrics metrics)
        {
            _productService = productService;
            _metrics = metrics;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProduct(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.GetProductByIdCounter);

            return product;
        }
    }
}
