using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public sealed class Product
    {
        [Key]
        public long ProductId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        [ForeignKey(nameof(CategoryId))]
        public long CategoryId { get; set; }
        public Category? Category { get; set; }
        [Required]
        [ForeignKey(nameof(SupplierId))]
        public long SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public string Images { get; set; } = string.Empty;
    }
}
