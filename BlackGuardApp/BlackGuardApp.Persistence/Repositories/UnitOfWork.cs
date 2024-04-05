using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Persistence.AppContext;

namespace BlackGuardApp.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlackGADbContext _context;
        public UnitOfWork(BlackGADbContext context)
        {
            ProductRepository = new ProductRepository(context);
            _context = context;
        }
        public IProductRepository ProductRepository { get; }
        public void Dispose() => _context.Dispose();

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
