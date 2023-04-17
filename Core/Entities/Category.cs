using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public sealed class Category
    {
        [Key]
        public long CategoryId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Product>? Products { get; set; }
    }
}
