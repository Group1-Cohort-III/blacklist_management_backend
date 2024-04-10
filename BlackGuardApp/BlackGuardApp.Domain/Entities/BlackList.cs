using BlackGuardApp.Domain.Entities.SharedEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Domain.Entities
{
    public class BlackList : BaseEntity
    {   
        public string ProductId { get; set; }  
        public string BlacklistCriteriaId { get; set; }

        [ForeignKey("BlacklistCriteriaId")]
        public BlacklistCriteria BlacklistCriteria { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        
       
    }

}
