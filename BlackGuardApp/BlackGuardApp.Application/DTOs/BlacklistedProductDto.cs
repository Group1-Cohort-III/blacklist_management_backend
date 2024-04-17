using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.DTOs
{
    public class BlacklistedProductDto
    {  
        public string BlacklistId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string CriteriaName { get; set; }
        public string CriteriaDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }
      //  public string UserId { get; set; } 
                                         
    }

}
