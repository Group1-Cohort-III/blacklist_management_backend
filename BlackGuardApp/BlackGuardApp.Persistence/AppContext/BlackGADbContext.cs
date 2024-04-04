using BlackGuardApp.Domain.Entities;

namespace BlackGuardApp.Persistence.AppContext
{
    public class BlackGADbContext : IdentityDbContext<AppUser>
    {
        public BlackGADbContext(DbContextOptions<BlackGADbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<BlackListedProduct> BlacklistItems { get; set; }
        public DbSet<BlacklistHistory> BlacklistHistories { get; set; }
        public DbSet<BlacklistCriteria> BlacklistCriterias { get; set; }
       
    }
}
