namespace Core.Contracts.Controllers.Ratings
{
    public sealed record UpdateRatingRequest(Guid RatingId, long ProductId, int Value, string Comment);
}
