using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.DTOs
{
    public class BlacklistedProductsDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
