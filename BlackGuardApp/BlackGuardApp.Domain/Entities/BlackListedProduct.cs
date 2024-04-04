using BlackGuardApp.Domain.Entities.SharedEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Domain.Entities
{
    public class BlackListedProduct : BaseEntity
    {
        [ForeignKey("ProductId")]
        public string ProductId { get; set; }

        [ForeignKey("BlacklistCriteriaId")]
        public string BlacklistCriteriaId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public BlacklistCriteria BlacklistCriteria { get; set; }
        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
       
    }

}
