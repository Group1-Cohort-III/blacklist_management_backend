using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlackGuardApp.Application.Interfaces.Repositories;



namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(string id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task<List<Product>> FindProduct(Expression<Func<Product, bool>> condition);
    }
}
