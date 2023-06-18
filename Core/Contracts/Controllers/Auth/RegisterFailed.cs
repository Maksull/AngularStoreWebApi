namespace Core.Contracts.Controllers.Auth
{
    public sealed record RegisterFailed(IEnumerable<string> Errors);
}
