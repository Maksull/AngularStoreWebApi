namespace Core.Entities
{
    public sealed class CartLine
    {
        public long CartLineId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public long OrderId { get; set; }
    }
}
