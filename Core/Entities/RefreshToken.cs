namespace Core.Entities
{
    public sealed class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expired { get; set; }
    }
}
