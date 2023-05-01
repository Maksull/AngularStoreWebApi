using Core.Contracts.Controllers.Suppliers;
using FluentValidation;

namespace Core.Validators.Suppliers
{
    public sealed class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
    {
        public CreateSupplierRequestValidator()
        {
            RuleFor(s => s.Name).NotEmpty();
            RuleFor(s => s.City).NotEmpty();
        }

    }
}
