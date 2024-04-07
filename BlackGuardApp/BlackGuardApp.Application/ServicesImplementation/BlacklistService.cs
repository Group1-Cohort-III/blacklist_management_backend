using AutoMapper;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain;
using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.ServicesImplementation
{
    public class BlacklistService : IBlacklistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BlacklistService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlacklistService(IUnitOfWork unitOfWork, 
                               IMapper mapper, 
                               ILogger<BlacklistService> logger,
                               IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<PageResult<List<BlacklistedProductsDto>>> GetBlacklistedProductsAsync(int page, int pageSize)
        {
            try
            {
                var blacklistedItems = await _unitOfWork.BlacklistRepository.GetAllAsync(); 
                var mappedItems = _mapper.Map<List<BlacklistedProductsDto>>(blacklistedItems);

                var pageResult = await Pagination<BlacklistedProductsDto>.GetPager(mappedItems, pageSize, page,
                    item => item.ItemName, item => item.ItemId.ToString());

                var result = new PageResult<List<BlacklistedProductsDto>>
                {
                    Data = pageResult.Data.ToList(), 
                    TotalPageCount = pageResult.TotalPageCount,
                    CurrentPage = pageResult.CurrentPage,
                    PerPage = pageResult.PerPage,
                    TotalCount = pageResult.TotalCount
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get blacklisted products.");
                throw; 
            }
        }


        public async Task<ApiResponse<BlacklistedProductDto>> GetBlacklistedProductAsync(string blacklistId)
        {
            try
            {
                BlackList blacklist = await _unitOfWork.BlacklistRepository.GetBlacklistByIdAsync(blacklistId);
                if (blacklist == null)
                    return ApiResponse<BlacklistedProductDto>.Failed(new List<string> { "Blacklisted product not found" });

                var mappedItem = _mapper.Map<BlacklistedProductDto>(blacklist);
                return ApiResponse<BlacklistedProductDto>.Success(mappedItem, "Blacklisted product retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get blacklisted product with ID {blacklistId}.");
                return ApiResponse<BlacklistedProductDto>.Failed(new List<string> { $"Failed to get blacklisted product with ID {blacklistId}" });
            }
        }

        public async Task<ApiResponse<bool>> RemoveFromBlacklistAsync(string blacklistId, string reason)
        {
            try
            {
                if (string.IsNullOrEmpty(blacklistId))
                {
                    return ApiResponse<bool>.Failed(new List<string> { "Invalid input" });
                }

                string userName = GetUserNameFromSession();
               
                var blacklistedItem = await _unitOfWork.BlacklistRepository.GetByIdAsync(blacklistId);
                if (blacklistedItem == null)
                {
                    return ApiResponse<bool>.Failed(new List<string> { "Blacklisted product not found" });
                }

                blacklistedItem.IsDeleted = true;

                BlacklistHistory blacklistHistory = new BlacklistHistory
                {
                    BlackListId = blacklistId,
                    Reason = reason,
                    CreatedBy = userName,
                    Status = "Removed"
                };

                _unitOfWork.BlacklistRepository.Update(blacklistedItem);
                await _unitOfWork.BlacklistHistoryRepository.AddAsync(blacklistHistory);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Blacklisted product with ID {blacklistId} removed.");

                return ApiResponse<bool>.Success(true, $"Product removed successfully from Blacklist", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to remove blacklisted product with ID {blacklistId}.");
                return ApiResponse<bool>.Failed(new List<string> { $"Failed to remove blacklisted product." });
            }
        }


        public async Task<ApiResponse<bool>> BlacklistProductAsync(string productId, string blacklistCriteriaId, string reason)
        {
            try
            {
               string userName = GetUserNameFromSession();
                if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(blacklistCriteriaId))
                {
                    return ApiResponse<bool>.Failed(new List<string> { "Invalid input" });
                }
                var blacklist = new BlackList
                {
                    ProductId = productId,
                    BlacklistCriteriaId = blacklistCriteriaId,
                    CreatedBy = userName,
                };
                await _unitOfWork.BlacklistRepository.AddAsync(blacklist);
                var blacklistHistory = new BlacklistHistory
                {
                    BlackListId = blacklist.Id,
                    Reason = reason,
                    CreatedBy = userName,
                    Status = "Added"
                };


                await _unitOfWork.BlacklistHistoryRepository.AddAsync(blacklistHistory);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Item with ID {productId} blacklisted. Category: {blacklistCriteriaId}, Reason: {reason}");

                return ApiResponse<bool>.Success(true, $"Product blacklisted successfully", 200);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Failed to blacklist product with ID {productId}. Database update error.");
                return ApiResponse<bool>.Failed(new List<string> { $"Failed to blacklist product" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to blacklist product with ID {productId}.");
                return ApiResponse<bool>.Failed(new List<string> { $"Failed to blacklist product" });
            }
        }





        private string GetUserNameFromSession()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var firstNameClaim = httpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            var surnameClaim = httpContext?.User?.FindFirst(ClaimTypes.Surname)?.Value;

            if (string.IsNullOrEmpty(firstNameClaim) || string.IsNullOrEmpty(surnameClaim))
            {
                throw new InvalidOperationException("First name or surname not found.");
            }

            string fullName = $"{firstNameClaim} {surnameClaim}";

            return fullName;
        }
    }
}
