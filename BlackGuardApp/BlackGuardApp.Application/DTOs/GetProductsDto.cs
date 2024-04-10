using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.DTOs
{
    public class GetProductsDto
    {
        public List<ProductResponseDto> Product { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPageCount { get; set; }
        public int TotalCount
        {
            get; set;
        }
    }
}
