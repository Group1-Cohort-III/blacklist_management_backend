using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.Interfaces.Services
{
    public interface IBlacklistService
    {
      
        Task<ApiResponse<bool>> BlacklistProductAsync(string productId, string blacklistCriteriaId, string reason, string userId);
        Task<ApiResponse<BlacklistedProductDto>> GetBlacklistedProductAsync(string id);
        Task<PageResult<List<BlacklistedProductsDto>>> GetBlacklistedProductsAsync(int page, int pageSize);
        Task<ApiResponse<bool>> RemoveFromBlacklistAsync(string id, string reason, string userId);
    }
}
