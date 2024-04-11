using BlackGuardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.Interfaces.Services
{
    public interface IBlacklistCriteriaService
    {
        Task<string> AddBlackListCriteria(string categoryName, string categoryDescription);
        Task<BlacklistCriteria> GetBlacklistCriteriaAsync(string creteriaId);
        Task<List<BlacklistCriteria>> GetBlacklistCriteriasAsync();
    }
}
