using Core.Entities;
using Core.Mediator.Queries.Products;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Handlers.Products
{
    public sealed class GetProductsByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductService _productService;

        public GetProductsByIdHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProduct(request.Id);
        }
    }
}
