using Microsoft.AspNetCore.Identity;

namespace LinkUp.Core.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
    }
}