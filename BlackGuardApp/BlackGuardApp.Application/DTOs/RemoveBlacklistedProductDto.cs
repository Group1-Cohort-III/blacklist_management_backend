using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.DTOs
{
    public class RemoveBlacklistedProductDto
    {
        public string Id { get; set; }
        public string Reason { get; set; }
    }
}
