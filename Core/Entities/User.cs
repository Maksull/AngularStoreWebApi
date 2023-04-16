using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public sealed class User : IdentityUser
    {
        [MaxLength(256)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(256)]
        public string LastName { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpired { get; set; }
    }
}
