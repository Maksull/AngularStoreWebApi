namespace Core.Contracts.Controllers.Auth
{
    public sealed record RefreshTokenRequest(string Token, DateTime Expired);
}
