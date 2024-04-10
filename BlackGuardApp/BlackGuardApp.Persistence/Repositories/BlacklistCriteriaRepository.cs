using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Domain.Entities;
using BlackGuardApp.Persistence.AppContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Persistence.Repositories
{
    public class BlacklistCriteriaRepository : GenericRepository<BlacklistCriteria>, IBlacklistCriteriaRepository
    {

        private readonly BlackGADbContext _blackGADbContext;

        public BlacklistCriteriaRepository(BlackGADbContext blackGADbContext) : base(blackGADbContext)
        {
            _blackGADbContext = blackGADbContext;
        }


    }
}
