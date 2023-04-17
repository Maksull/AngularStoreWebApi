using Core.Entities;

namespace Core.Contracts.Controllers.Auth
{
    public sealed record JwtResponse(string Jwt, RefreshToken RefreshToken);
}
