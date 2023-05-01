using Core.Entities;
using Core.Mediator.Commands.Products;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Products
{
    public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product?>
    {
        private readonly IProductService _productService;

        public UpdateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Product?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            return await _productService.UpdateProduct(request.Product);
        }
    }
}
