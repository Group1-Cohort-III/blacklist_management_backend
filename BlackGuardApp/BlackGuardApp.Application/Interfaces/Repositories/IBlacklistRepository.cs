using BlackGuardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IBlacklistRepository : IGenericRepository<BlackList>
    {
        Task<List<BlackList>> GetBlacklistIncludingAsync();
        Task<BlackList> GetBlacklistIncludingByIdAsync(string blacklistId);
        Task<bool> GetByProductAsync(string productId);
    }
}
