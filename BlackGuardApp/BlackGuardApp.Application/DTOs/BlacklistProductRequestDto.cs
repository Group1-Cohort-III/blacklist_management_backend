using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.DTOs
{
    public class BlacklistProductRequestDto
    {
       
        public string ProductId { get; set; }
        public string CriteriaId { get; set; }
        public string Reason { get; set; }
        public string UserId { get; set; }
    }
}
