using Core.Contracts.Controllers.Suppliers;
using FluentValidation;

namespace Core.Validators.Suppliers
{
    public sealed class UpdateSupplierRequestValidator : AbstractValidator<UpdateSupplierRequest>
    {
        public UpdateSupplierRequestValidator()
        {
            RuleFor(s => s.SupplierId).GreaterThanOrEqualTo(1);
            RuleFor(s => s.Name).NotEmpty();
            RuleFor(s => s.City).NotEmpty();
        }

    }
}
