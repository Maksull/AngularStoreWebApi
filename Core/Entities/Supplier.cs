using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public sealed class Supplier
    {
        [Key]
        public long SupplierId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        public IEnumerable<Product>? Products { get; set; }
    }
}
