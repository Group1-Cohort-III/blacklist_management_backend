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
        [ForeignKey("BlackListProductId")]
        public string BlackListProductId { get; set; }

        public string? Reason { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; } 
        public string Status { get; set; }

        public BlackListedProduct BlackListedProduct { get; set; }
    }
}
