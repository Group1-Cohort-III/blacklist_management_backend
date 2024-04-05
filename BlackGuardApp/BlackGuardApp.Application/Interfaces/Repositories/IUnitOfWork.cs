namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {

        IProductRepository ProductRepository { get; }
        IAppUserRepository AppUserRepository { get; }
        Task<int> SaveChangesAsync();
        void Dispose();

    }
}
