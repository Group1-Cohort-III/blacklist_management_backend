using BlackGuardApp.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace BlackGuardApp.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public UserRoles Roles { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}
