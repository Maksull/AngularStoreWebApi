using Core.Contracts.Controllers.Auth;
using FluentValidation;

namespace Core.Validators.Auth
{
    public sealed class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(r => r.Token).NotEmpty();
            RuleFor(r => r.Expired).NotEmpty();
        }
    }
}
