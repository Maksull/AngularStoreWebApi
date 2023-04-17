using Core.Contracts.Controllers.Auth;
using FluentValidation;

namespace Core.Validators.Auth
{
    public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(r => r.FirstName).NotEmpty();
            RuleFor(r => r.LastName).NotEmpty();
            RuleFor(r => r.Username).NotEmpty();
            RuleFor(r => r.Email).Matches("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$");
            RuleFor(r => r.Password).MatchesPassword();
            RuleFor(r => r.ConfirmPassword).Equal(r => r.Password)
                .WithMessage("'ConfirmPassword' must be equal to password");
        }
    }
}
