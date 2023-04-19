using Core.Entities;
using Core.Mediator.Commands.Products;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Products
{
    public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductService _productService;

        public CreateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.CreateProduct(request.Product);

            return product;
        }
    }
}
