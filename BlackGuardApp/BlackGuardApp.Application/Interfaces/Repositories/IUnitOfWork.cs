namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
