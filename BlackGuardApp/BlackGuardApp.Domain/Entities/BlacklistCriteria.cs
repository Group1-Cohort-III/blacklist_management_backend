using BlackGuardApp.Domain.Entities.SharedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Domain.Entities
{
    public class BlacklistCriteria : BaseEntity
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
    }
}
