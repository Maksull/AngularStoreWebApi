using Core.Contracts.Controllers.Categories;
using FluentValidation;

namespace Core.Validators.Categories
{
    public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }

    }
}
