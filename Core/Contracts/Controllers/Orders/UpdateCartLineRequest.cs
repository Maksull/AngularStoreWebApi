namespace Core.Contracts.Controllers.Orders
{
    public sealed record UpdateCartLineRequest(long CartLineId, long ProductId, int Quantity, long OrderId);
}
