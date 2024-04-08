using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.DTOs
{
    public class PaginationDto
    {
       public int Page { get; set; } = 1;
       public int PageSize { get; set; } = 10;
    }
}
