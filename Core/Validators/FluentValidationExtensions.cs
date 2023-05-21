using FluentValidation;
using System.Text.RegularExpressions;

namespace Core.Validators
{
    internal static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MatchesPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(password => Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W)[A-Za-z\d\W]{6,}$"))
                              .WithMessage("Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
        }
        public static IRuleBuilderOptions<T, string> MatchesUkrainianPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(phoneNumber => Regex.IsMatch(phoneNumber, @"^\+380\d{9}$"))
                              .WithMessage("Please enter a valid Ukrainian phone number starting with '+380'.");
        }
    }
}
