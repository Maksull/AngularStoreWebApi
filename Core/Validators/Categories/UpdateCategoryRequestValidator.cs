using Core.Contracts.Controllers.Categories;
using FluentValidation;

namespace Core.Validators.Categories
{
    public sealed class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(c => c.CategoryId).GreaterThanOrEqualTo(1);
            RuleFor(c => c.Name).NotEmpty();
        }

    }
}
