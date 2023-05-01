namespace Core.Contracts.Controllers.Orders
{
    public sealed record CreateOrderRequest(string Name, string Email, string Address, string City, string Country, string Zip, IEnumerable<CreateCartLineRequest>? Lines);
}
