namespace WebApi.Models
{
    public sealed class CartLine
    {
        public long CartLineId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }
}
