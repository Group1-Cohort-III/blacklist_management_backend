using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("get-blacklisted-products")]
        public async Task<IActionResult> GetBlacklistedProducts([FromQuery] PaginationDto dto )
        {
            var response = await _blacklistService.GetBlacklistedProductsAsync(dto.Page, dto.PageSize, dto.FilterValue, dto.Date);
            return Ok(response);
        }

        [Authorize(Policy = "BlackListAdminPolicy")]
        [HttpGet("get-blacklisted-product")]
        public async Task<IActionResult> GetBlacklistedProduct(string blacklistId)
        {
            var response = await _blacklistService.GetBlacklistedProductAsync(blacklistId);
          
            return Ok(response);
        }

        [Authorize(Policy = "BlackListAdminPolicy")]
        [HttpPut("remove")]
        public async Task<IActionResult> RemoveBlacklistedProduct([FromBody] RemoveBlacklistedProductDto requestDto )
        {
          //  var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");

            var response = await _blacklistService.RemoveFromBlacklistAsync(requestDto.Id, requestDto.Reason, requestDto.userId);
            if (!response.Succeeded)
                return NotFound(response);

            return Ok(response);
        }

        [Authorize(Policy = "BlackListAdminPolicy")]
        [HttpPost("blacklist-product")]
        public async Task<IActionResult> BlacklistProduct([FromBody] BlacklistProductRequestDto requestDto)
        {
          //  var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");

            var response = await _blacklistService.BlacklistProductAsync(requestDto.ProductId, requestDto.CriteriaId, requestDto.Reason, requestDto.UserId);
            return Ok(response);
        }

       
    }
}

         
 
