using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.DTOs
{
    public class BlacklistedProductsDto
    {
        public string blacklistId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string CriteriaName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
