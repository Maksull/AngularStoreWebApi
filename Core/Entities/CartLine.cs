using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public sealed class CartLine
    {
        [Key]
        public long CartLineId { get; set; }
        [Required]
        [ForeignKey(nameof(ProductId))]
        public long ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [ForeignKey(nameof(OrderId))]
        public long OrderId { get; set; }
    }
}
