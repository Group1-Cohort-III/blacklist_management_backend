﻿using BlackGuardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.Interfaces.Repositories
{
    public interface IBlacklistHistoryRepository : IGenericRepository<BlacklistHistory>
    {
        Task<BlacklistHistory> GetHistoryByBlacklistIdAsync(string blacklistId);
    }
}
