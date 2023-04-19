namespace Core.Contracts.Controllers.Orders
{
    public sealed record UpdateOrderRequest(long OrderId, string Name, string Email, string Address, string City, string Country, string Zip, bool IsShipped, IEnumerable<UpdateCartLineRequest>? Lines);
}
