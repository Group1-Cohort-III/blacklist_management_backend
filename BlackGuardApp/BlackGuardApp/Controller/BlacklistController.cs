using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace BlackGuardApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        private readonly IBlacklistService _blacklistService;
        private readonly IBlacklistCriteriaService _blacklistCriteriaService;
        public BlacklistController(IBlacklistService blacklistService, IBlacklistCriteriaService blacklistCriteriaService)
        {
            _blacklistService = blacklistService;
            _blacklistCriteriaService = blacklistCriteriaService;
        }

        [HttpGet("getBlacklistedProducts")]
        public async Task<IActionResult> GetBlacklistedProducts([FromQuery] PaginationDto dto )
        {
            var response = await _blacklistService.GetBlacklistedProductsAsync(dto.Page, dto.PageSize, dto.FilterValue, dto.Date);
            return Ok(response);
        }

        [HttpGet("getBlacklistedProduct")]
        public async Task<IActionResult> GetBlacklistedProduct(string blacklistId)
        {
            var response = await _blacklistService.GetBlacklistedProductAsync(blacklistId);
            if (!response.Succeeded)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("remove")]
        public async Task<IActionResult> RemoveBlacklistedProduct([FromBody] RemoveBlacklistedProductDto requestDto )
        {
          //  var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");

            var response = await _blacklistService.RemoveFromBlacklistAsync(requestDto.Id, requestDto.Reason, requestDto.userId);
            if (!response.Succeeded)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("blacklistProduct")]
        public async Task<IActionResult> BlacklistProduct([FromBody] BlacklistProductRequestDto requestDto)
        {
          //  var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");

            var response = await _blacklistService.BlacklistProductAsync(requestDto.ProductId, requestDto.CriteriaId, requestDto.Reason, requestDto.UserId);
            return Ok(response);
        }

        [HttpPost("AddBlacklistCriteria")]
        public async Task<IActionResult> AddBlacklistCriteria()
        {
            var response = await _blacklistCriteriaService.AddCategories();
            return Ok(response);
        }
    }
}

         
 