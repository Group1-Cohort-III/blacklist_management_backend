using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using BlackGuardApp.Domain.Entities.SharedEntities;
namespace BlackGuardApp.Domain.Entities
{
   

    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public bool IsBlacklisted { get; set; }
        public string ProductDescription { get; set; }
    }

}
