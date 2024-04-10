using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlackGuardApp.Persistence.AppContext
{
    public class BlackGADbContext : IdentityDbContext<AppUser>
    {
        public BlackGADbContext(DbContextOptions<BlackGADbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<BlackList> BlackLists { get; set; }
        public DbSet<BlacklistHistory> BlacklistHistories { get; set; }
        public DbSet<BlacklistCriteria> BlacklistCriterias { get; set; }

    }
}
