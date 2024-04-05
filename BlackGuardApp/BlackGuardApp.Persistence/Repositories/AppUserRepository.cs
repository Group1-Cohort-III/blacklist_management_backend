using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Domain.Entities;
using BlackGuardApp.Persistence.AppContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Persistence.Repositories
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(BlackGADbContext blackGADbContext) : base(blackGADbContext)
        {
        }

        public async Task<AppUser> GetUserById(string userId)
        {
            return await GetByIdAsync(userId);
        }

        public async Task <IEnumerable<AppUser>> GetAllUsers()
        {
            return await  GetAllAsync();
        }

        public async Task <IEnumerable<AppUser>> FindUsers(Expression<Func<AppUser, bool>> predicate)
        {
            return await FindAsync(predicate);
        }
        public async Task AddUserAsync(AppUser user)
        {
            await Task.Run(() => AddAsync(user));
        }

        public async Task UpdateUserAsync(AppUser user)
        {
            await Task.Run(() => Update(user));
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                await Task.Run(() => DeleteAsync(user));
            }
        }

        
    }
}
