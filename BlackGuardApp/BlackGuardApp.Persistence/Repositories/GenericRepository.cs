using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Persistence.AppContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlackGuardApp.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BlackGADbContext _context;

        public GenericRepository(BlackGADbContext context) => _context = context;

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void DeleteAsync(T entity) => _context.Set<T>().Remove(entity);

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression) => await _context.Set<T>().Where(expression).ToListAsync();

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression) => await _context.Set<T>().AnyAsync(expression);

        public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(string Id) => await _context.Set<T>().FindAsync(Id);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Update(T entity) => _context.Set<T>().Update(entity);
    }
}
