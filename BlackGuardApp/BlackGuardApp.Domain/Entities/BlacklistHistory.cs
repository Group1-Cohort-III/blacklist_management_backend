using BlackGuardApp.Domain.Entities.SharedEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Domain.Entities
{
    public class BlacklistHistory : BaseEntity
    {    
        public string BlackListId { get; set; }

        public string? Reason { get; set; }
        public string Status { get; set; }

        [ForeignKey("BlackListId")]
        public BlackList BlackList { get; set; }
    }
}
