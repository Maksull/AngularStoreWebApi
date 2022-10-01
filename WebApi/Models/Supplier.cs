using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public sealed class Supplier
    {
        [BindNever]
        public long SupplierId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; } = string.Empty;
        [BindNever]
        public IEnumerable<Product>? Products { get; set; }
    }
}
