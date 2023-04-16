using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public sealed class Order
    {
        [BindNever]
        public long OrderId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; } = string.Empty;
        [Required(ErrorMessage = "Zip is required")]
        public string Zip { get; set; } = string.Empty;
        [BindNever]
        public bool IsShipped { get; set; }
        [BindNever]
        public IEnumerable<CartLine>? Lines { get; set; }
    }
}
