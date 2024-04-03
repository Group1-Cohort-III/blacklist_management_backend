using System.Linq.Expressions;

namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string Id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        void Update(T entity);
        void DeleteAsync(T entity);
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
    }
}
