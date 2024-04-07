using Azure;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Domain;
using Microsoft.AspNetCore.Mvc;



namespace BlackGuardApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        private readonly IBlacklistService _blacklistService;

        public BlacklistController(IBlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }

        [HttpGet("blacklistedProducts")]
        public async Task<IActionResult> GetBlacklistedProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _blacklistService.GetBlacklistedProductsAsync(page, pageSize);
            return Ok(response);
        }

        [HttpGet("blacklistedProduct")]
        public async Task<IActionResult> GetBlacklistedProduct(string id)
        {
            var response = await _blacklistService.GetBlacklistedProductAsync(id);
            if (!response.Succeeded)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("remove")]
        public async Task<IActionResult> RemoveBlacklistedItem(string id, [FromBody] string reason)
        {
            var response = await _blacklistService.RemoveFromBlacklistAsync(id, reason);
            if (!response.Succeeded)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("blacklistProduct")]
        public async Task<IActionResult> BlacklistProduct([FromBody] BlacklistProductRequestDto requestDto)
        {
            var response = await _blacklistService.BlacklistProductAsync(requestDto.ProductId, requestDto.CriteriaId, requestDto.Reason);
            return Ok(response);
        }
    }
}

         
 