using Core.Contracts.Controllers.Products;
using FluentValidation;

namespace Core.Validators.Products
{
    public sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(p => p.ProductId).GreaterThanOrEqualTo(1);
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.Price).GreaterThan(0);
            RuleFor(p => p.CategoryId).GreaterThanOrEqualTo(1);
            RuleFor(p => p.SupplierId).GreaterThanOrEqualTo(1);
        }

    }
}
