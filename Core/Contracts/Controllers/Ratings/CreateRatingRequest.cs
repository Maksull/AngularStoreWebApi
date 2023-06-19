namespace Core.Contracts.Controllers.Ratings
{
    public sealed record CreateRatingRequest(long ProductId, int Value, string Comment);
}
