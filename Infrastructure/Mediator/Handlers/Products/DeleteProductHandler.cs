using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Products;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Products
{
    public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Product?>
    {
        private readonly IProductService _productService;
        private readonly IMetrics _metrics;

        public DeleteProductHandler(IProductService productService, IMetrics metrics)
        {
            _productService = productService;
            _metrics = metrics;
        }

        public async Task<Product?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.DeleteProduct(request.Id);

            _metrics.Measure.Counter.Increment(MetricsRegistry.DeleteProductCounter);

            return product;
        }
    }
}
