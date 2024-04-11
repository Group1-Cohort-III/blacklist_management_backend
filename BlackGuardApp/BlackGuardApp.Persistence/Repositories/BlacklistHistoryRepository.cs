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
    public class BlacklistHistoryRepository : GenericRepository<BlacklistHistory>, IBlacklistHistoryRepository
    {

        private readonly BlackGADbContext _blackGADbContext;

    public BlacklistHistoryRepository(BlackGADbContext blackGADbContext) : base(blackGADbContext)
    {
        _blackGADbContext = blackGADbContext;
    }

        public async Task<BlacklistHistory> GetHistoryByBlacklistIdAsync(string blacklistId)
        {
            return await _blackGADbContext.BlacklistHistories
                .FirstOrDefaultAsync(bl => bl.BlackListId.Equals(blacklistId));
        }

    }
}

