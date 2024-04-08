using Azure;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Domain;
using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



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

        [HttpGet("getBlacklistedProducts")]
        public async Task<IActionResult> GetBlacklistedProducts([FromQuery] PaginationDto dto )
        {
            var response = await _blacklistService.GetBlacklistedProductsAsync(dto.Page, dto.PageSize);
            return Ok(response);
        }

        [HttpGet("getBlacklistedProduct")]
        public async Task<IActionResult> GetBlacklistedProduct(BlacklistedProdRequestDto requestDto)
        {
            var response = await _blacklistService.GetBlacklistedProductAsync(requestDto.Id);
            if (!response.Succeeded)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("remove")]
        public async Task<IActionResult> RemoveBlacklistedProduct([FromBody] RemoveBlacklistedProductDto requestDto )
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");

            var response = await _blacklistService.RemoveFromBlacklistAsync(requestDto.Id, requestDto.Reason, userId);
            if (!response.Succeeded)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("blacklistProduct")]
        public async Task<IActionResult> BlacklistProduct([FromBody] BlacklistProductRequestDto requestDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");

            var response = await _blacklistService.BlacklistProductAsync(requestDto.ProductId, requestDto.CriteriaId, requestDto.Reason, userId);
            return Ok(response);
        }
    }
}

         
 