using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlackGuardApp.Persistence.AppContext
{
    public class BlackGADbContext : IdentityDbContext<AppUser>
    {
        public BlackGADbContext(DbContextOptions<BlackGADbContext> options) : base(options) { }
    }
}
