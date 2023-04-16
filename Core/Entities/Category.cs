using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public sealed class Category
    {
        [BindNever]
        public long CategoryId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; } = string.Empty;
        [BindNever]
        public IEnumerable<Product>? Products { get; set; }
    }
}
