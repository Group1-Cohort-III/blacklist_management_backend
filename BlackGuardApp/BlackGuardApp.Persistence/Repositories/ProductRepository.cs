using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Domain.Entities;
using BlackGuardApp.Persistence.AppContext;


namespace BlackGuardApp.Persistence.Repositories
{
    public class ProductRepository: GenericRepository<Product>, IProductRepository
    {
        private readonly BlackGADbContext _blackGADbContext;

        public ProductRepository(BlackGADbContext blackGADbContext) : base(blackGADbContext)
        {
            _blackGADbContext = blackGADbContext;
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await GetAllAsync();
        }
        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await AddAsync(product);
        }


        public async Task UpdateProductAsync(Product product)
        {
            Update(product);
            await Task.CompletedTask; // Just a placeholder for asynchronous signature
        }

        public async Task DeleteProductAsync(Product product)
        {
            DeleteAsync(product);
            await _blackGADbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> FindProductAsync(Expression<Func<Product, bool>> condition)
        {
            return await FindAsync(condition);
        }
    }
    

}
