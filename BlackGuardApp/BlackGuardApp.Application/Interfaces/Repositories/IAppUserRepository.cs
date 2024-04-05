using BlackGuardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IAppUserRepository : IGenericRepository<AppUser>
    {
        Task<AppUser> GetUserById(string userId);
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<IEnumerable<AppUser>> FindUsers(Expression<Func<AppUser, bool>> predicate);
        Task AddUserAsync(AppUser user);
        Task UpdateUserAsync(AppUser user);
        Task DeleteUserAsync(string userId);
    }
}
