using AutoMapper;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain;
using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
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
   
        private readonly IUserAdminServices _userAdminServices;
        public BlacklistService(IUnitOfWork unitOfWork, 
                               IMapper mapper, 
                               ILogger<BlacklistService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
       
        }


        public async Task<PageResult<List<BlacklistedProductsDto>>> GetBlacklistedProductsAsync(int page, int pageSize, string? filterValue, string? dateString)
        {
            try
            {
                List<BlackList> blacklistedItems = await _unitOfWork.BlacklistRepository.GetBlacklistIncludingAsync();
                blacklistedItems = blacklistedItems.Where(item => item.IsDeleted).ToList();
                var mappedItems = _mapper.Map<List<BlacklistedProductsDto>>(blacklistedItems);

                if (!string.IsNullOrEmpty(filterValue))
                {
                    mappedItems = ApplyFilter(mappedItems, filterValue);
                }

                if (!string.IsNullOrEmpty(dateString))
                {
                    DateTime date;
                    if (DateTime.TryParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ||
                        DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ||
                        DateTime.TryParseExact(dateString, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        mappedItems = ApplyDateFilter(mappedItems, date);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid date format. Supported formats are MM/dd/yyyy, dd/MM/yyyy, and yyyy/MM/dd.");
                    }
                }

                var pageResult = await Pagination<BlacklistedProductsDto>.GetPager(mappedItems, pageSize, page,
                    item => item.ProductName, item => item.blacklistId.ToString());

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
                BlackList blacklist = await _unitOfWork.BlacklistRepository.GetBlacklistIncludingByIdAsync(blacklistId);
              
                if (blacklist == null || blacklist.IsDeleted)
                    return ApiResponse<BlacklistedProductDto>.Failed(new List<string> { "Blacklisted product not found" });
        
                string reason = await GetBlacklistReasonAsync(blacklistId);
                var mappedItem = _mapper.Map<BlacklistedProductDto>(blacklist);
                mappedItem.Reason = reason;

                return ApiResponse<BlacklistedProductDto>.Success(mappedItem, "Blacklisted product retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get blacklisted product with ID {blacklistId}.");
                return ApiResponse<BlacklistedProductDto>.Failed(new List<string> { $"Failed to get blacklisted product with ID {blacklistId}" });
            }
        }

        public async Task<ApiResponse<bool>> RemoveFromBlacklistAsync(string blacklistId, string reason, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(blacklistId))
                {
                    return ApiResponse<bool>.Failed(new List<string> { "Invalid input" });
                }


               
                var blacklistedItem = await _unitOfWork.BlacklistRepository.GetByIdAsync(blacklistId);
                if (blacklistedItem == null)
                {
                    return ApiResponse<bool>.Failed(new List<string> { "Blacklisted product not found" });
                }
                Product product = await _unitOfWork.ProductRepository.GetByIdAsync(blacklistedItem.ProductId);
                blacklistedItem.IsDeleted = true;

                BlacklistHistory blacklistHistory = new BlacklistHistory
                {
                    BlackListId = blacklistId,
                    Reason = reason,
                    CreatedBy = userId,
                    Status = "Removed"
                };
                product.IsBlacklisted = true;
                _unitOfWork.BlacklistRepository.Update(blacklistedItem);
                await _unitOfWork.BlacklistHistoryRepository.AddAsync(blacklistHistory);
                _unitOfWork.ProductRepository.Update(product);
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


        public async Task<ApiResponse<bool>> BlacklistProductAsync(string productId, string blacklistCriteriaId, string reason,string userId)
        {
            try
            {
              
                if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(blacklistCriteriaId))
                {
                    return ApiResponse<bool>.Failed(new List<string> { "Invalid input" });
                }
               Product product =  await _unitOfWork.ProductRepository.GetByIdAsync(productId);
              
                var blacklist = new BlackList
                {
                    ProductId = productId,
                    BlacklistCriteriaId = blacklistCriteriaId,
                    CreatedBy = userId,
                };
                await _unitOfWork.BlacklistRepository.AddAsync(blacklist);
                var blacklistHistory = new BlacklistHistory
                {
                    BlackListId = blacklist.Id,
                    Reason = reason,
                    CreatedBy = userId,
                    Status = "Added"
                };
                product.IsBlacklisted = true;
                await _unitOfWork.BlacklistHistoryRepository.AddAsync(blacklistHistory);
                await _unitOfWork.ProductRepository.UpdateProductAsync(product);
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


        private async Task<string> GetBlacklistReasonAsync(string blacklistId)
        {
            // Assuming BlacklistHistory has a property called 'Reason'
            var blacklistHistory = await  _unitOfWork.BlacklistHistoryRepository.GetByIdAsync(blacklistId);
            if (blacklistHistory.Reason != null)
            {
                return blacklistHistory.Reason;
            }
            else
            {
                return null;
            }
        }

        private List<BlacklistedProductsDto> ApplyFilter(List<BlacklistedProductsDto> items, string? filterValue)
        {
            return items.Where(item =>
                item.ProductName.Contains(filterValue) ||
                item.CriteriaName.Contains(filterValue)
            ).ToList();
        }

        private List<BlacklistedProductsDto> ApplyDateFilter(List<BlacklistedProductsDto> items, DateTime date)
        {
            return items.Where(item => item.CreatedAt >= date
            ).ToList();
        }


    }
}
