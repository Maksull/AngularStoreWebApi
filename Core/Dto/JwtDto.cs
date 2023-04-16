using Core.Entities;

namespace Core.Dto
{
    public sealed class JwtDto
    {
        public required string Jwt { get; set; }
        public RefreshToken? RefreshToken { get; set; }
    }
}
