using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public sealed class Order
    {
        [Key]
        public long OrderId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string Zip { get; set; } = string.Empty;
        [Required]
        public bool IsShipped { get; set; }
        public IEnumerable<CartLine>? Lines { get; set; }
    }
}
