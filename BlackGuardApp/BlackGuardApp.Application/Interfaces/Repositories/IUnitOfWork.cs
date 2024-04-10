namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {

        IProductRepository ProductRepository { get; }
        IAppUserRepository AppUserRepository { get; }
        IBlacklistHistoryRepository BlacklistHistoryRepository { get; }
        IBlacklistRepository BlacklistRepository { get; }
        IBlacklistCriteriaRepository BlacklistCriteriaRepository { get; }

        Task<int> SaveChangesAsync();
        void Dispose();

    }
}
