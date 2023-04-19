using Core.Contracts.Controllers.Orders;
using FluentValidation;

namespace Core.Validators.Orders
{
    public sealed class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidator()
        {
            RuleFor(o => o.OrderId).GreaterThanOrEqualTo(1);
            RuleFor(o => o.Name).NotEmpty();
            RuleFor(o => o.Email).NotEmpty();
            RuleFor(o => o.Address).NotEmpty();
            RuleFor(o => o.City).NotEmpty();
            RuleFor(o => o.Country).NotEmpty();
            RuleFor(o => o.Zip).NotEmpty();
            RuleFor(o => o.IsShipped).NotEmpty();
            RuleFor(o => o.Lines).NotEmpty();
        }
    }
}
