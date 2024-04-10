using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Persistence.AppContext;

namespace BlackGuardApp.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlackGADbContext _context;
        public UnitOfWork(BlackGADbContext context)
        {
            _context = context;
            ProductRepository = new ProductRepository(context);
            AppUserRepository = new AppUserRepository(context);
            BlacklistRepository = new BlacklistRepository(context);
            BlacklistHistoryRepository = new BlacklistHistoryRepository(context);
            BlacklistCriteriaRepository = new BlacklistCriteriaRepository(context);

        }
        public IBlacklistCriteriaRepository BlacklistCriteriaRepository { get; internal set; }
        public IBlacklistHistoryRepository BlacklistHistoryRepository { get; internal set; }
        public IBlacklistRepository BlacklistRepository { get; internal set; }
        public IProductRepository ProductRepository { get; }
        public IAppUserRepository AppUserRepository { get; private set; }

        public void Dispose() => _context.Dispose();

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
