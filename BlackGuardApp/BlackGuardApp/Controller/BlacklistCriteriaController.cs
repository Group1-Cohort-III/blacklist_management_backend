using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlackGuardApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistCriteriaController : ControllerBase
    {
        private readonly IBlacklistCriteriaService _blacklistCriteriaService;
        public BlacklistCriteriaController(IBlacklistCriteriaService blacklistCriteriaService)
        {
            
            _blacklistCriteriaService = blacklistCriteriaService;
        }

        [HttpPost("get-blacklist-criterias")]
        public async Task<IActionResult> GetBlacklistCriterias()
        {
            var response = await _blacklistCriteriaService.GetBlacklistCriteriasAsync();
            return Ok(response);
        }

        [HttpPost("get-blacklist-criteria")]
        public async Task<IActionResult> GetBlacklistCriteria(string criteriaId )
        {
            var response = await _blacklistCriteriaService.GetBlacklistCriteriaAsync(criteriaId);
            return Ok(response);
        }

        [HttpPost("add-blacklist-criteria")]
        public async Task<IActionResult> AddBlacklistCriteria(CriteriaDto dto)
        {
            var response = await _blacklistCriteriaService.AddBlackListCriteria(dto.CategoryName, dto.CategoryDescription);
            return Ok(response);
        }
       
    }
}
