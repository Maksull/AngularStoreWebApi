using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public sealed class Product
    {
        [BindNever]
        public long ProductId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(8,2)")]
        [Required(ErrorMessage = "Price is required")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "CategoryId is required")]
        public long CategoryId { get; set; }
        [BindNever]
        public Category? Category { get; set; }
        [Required(ErrorMessage = "SupplierId is required")]
        public long SupplierId { get; set; }
        [BindNever]
        public Supplier? Supplier { get; set; }
    }
}
