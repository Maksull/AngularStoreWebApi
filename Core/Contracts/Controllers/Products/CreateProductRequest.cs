using Microsoft.AspNetCore.Http;

namespace Core.Contracts.Controllers.Products
{
    public sealed record CreateProductRequest(string Name, string Description, decimal Price, long CategoryId, long SupplierId, IFormFile Img);
}
