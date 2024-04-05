namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {

        IProductRepository ProductRepository { get; }
        Task<int> SaveChangesAsync();
        void Dispose();

    }
}
