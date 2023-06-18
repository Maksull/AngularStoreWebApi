﻿using App.Metrics;
using Core.Entities;
using Core.Mediator.Commands.Products;
using Infrastructure.Metrics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Products
{
    public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product?>
    {
        private readonly IProductService _productService;
        private readonly IMetrics _metrics;

        public UpdateProductHandler(IProductService productService, IMetrics metrics)
        {
            _productService = productService;
            _metrics = metrics;
        }

        public async Task<Product?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.UpdateProduct(request.Product);

            _metrics.Measure.Counter.Increment(MetricsRegistry.UpdateProductCounter);

            return product;
        }
    }
}
