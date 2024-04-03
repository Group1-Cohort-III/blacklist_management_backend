using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Persistence.AppContext;
using Microsoft.EntityFrameworkCore;

namespace BlackGuardApp.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlackGADbContext _context;
        public UnitOfWork(BlackGADbContext context)
        {
            _context = context;
        }
        public void Dispose() => _context.Dispose();

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
