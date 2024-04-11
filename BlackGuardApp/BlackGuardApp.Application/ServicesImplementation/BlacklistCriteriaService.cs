using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.ServicesImplementation
{
    public class BlacklistCriteriaService : IBlacklistCriteriaService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BlacklistCriteria> _logger;

        public BlacklistCriteriaService(IUnitOfWork unitOfWork, ILogger<BlacklistCriteria> logger)
        {
            
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<List<BlacklistCriteria>> GetBlacklistCriteriasAsync()
        {
            try
            {
                List<BlacklistCriteria> criterias = await _unitOfWork.BlacklistCriteriaRepository.GetAllAsync();
           
                return criterias;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get blacklist criterias.");
                throw;
            }
        }

        public async Task<BlacklistCriteria> GetBlacklistCriteriaAsync(string creteriaId)
        {
            try
            {
                BlacklistCriteria criteria = await _unitOfWork.BlacklistCriteriaRepository.GetByIdAsync(creteriaId);

                return criteria;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get blacklist criteria.");
                throw;
            }
        }

        public async Task<string> AddBlackListCriteria(string categoryName, string categoryDescription)
        {
            try
            {
                var criteria = new BlacklistCriteria();
                criteria.CategoryName = categoryName;
                criteria.CategoryDescription = categoryDescription;
                await _unitOfWork.BlacklistCriteriaRepository.AddAsync(criteria);
                await _unitOfWork.SaveChangesAsync();

                return "Added successfully";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add blacklist criteria.");
                throw;
            }
        }

       
    }
}

