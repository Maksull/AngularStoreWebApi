namespace Core.Contracts.Controllers.Orders
{
    public sealed record CreateCartLineRequest(long ProductId, int Quantity);
}
