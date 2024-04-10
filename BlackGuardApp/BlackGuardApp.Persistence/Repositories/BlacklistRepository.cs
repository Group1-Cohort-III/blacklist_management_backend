using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Domain.Entities;
using BlackGuardApp.Persistence.AppContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Persistence.Repositories
{
    public class BlacklistRepository : GenericRepository<BlackList>, IBlacklistRepository
    {

        private readonly BlackGADbContext _blackGADbContext;

        public BlacklistRepository(BlackGADbContext blackGADbContext) : base(blackGADbContext)
        {
            _blackGADbContext = blackGADbContext;
        }

        public async Task<BlackList> GetBlacklistByIdAsync(string blacklistId)
        {
            return await _blackGADbContext.BlackLists
                .Include(bl => bl.BlacklistCriteria)  
                .Include(bl => bl.Product)          
                .FirstOrDefaultAsync(bl => bl.Id == blacklistId);
        }

        public async Task<List<BlackList>> GetBlacklistAsync()
        {
            return await _blackGADbContext.BlackLists
                .Include(bl => bl.BlacklistCriteria)
                .Include(bl => bl.Product)
                .ToListAsync();
        }
    }
}
