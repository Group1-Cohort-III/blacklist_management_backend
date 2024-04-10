using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Domain.Entities;
namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<List<Product>> FindProductAsync(Expression<Func<Product, bool>> condition);
    }
}
