namespace Core.Contracts.Controllers.Auth
{
    public sealed record ConfirmResetPasswordFailed(IEnumerable<string> Errors);
}
