using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public sealed class Rating
    {
        [Key]
        public Guid RatingId { get; set; }
        [Required]
        [ForeignKey(nameof(ProductId))]
        public long ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        [Required]
        public int Value { get; set; }
        [Required]
        public string Comment { get; set; } = string.Empty;
    }
}
