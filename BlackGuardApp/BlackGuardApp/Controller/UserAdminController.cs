using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlackGuardApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        
            private readonly IUserAdminServices _userAdminServices;

            public UserAdminController(IUserAdminServices userAdminService)
            {
                _userAdminServices = userAdminService;
            }

            [HttpPost]
            [Route("createUser")]
            public async Task<IActionResult> CreateUser(string emailAddress)
            {
                try
                {
                    var result = await _userAdminServices.CreateUser(emailAddress);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error creating user: {ex.Message}");
                }
            }

            [HttpGet]
            [Route("GetUserByEmail")]
            public async Task<IActionResult> GetUserByEmail(string emailAddress)
            {
                var user = await _userAdminServices.GetUserByEmail(emailAddress);
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound("User not found.");
            }

            [HttpPut]
            [Route("updateUser")]
            public async Task<IActionResult> UpdateUser(AppUserDtos user)
            {
                try
                {
                    var result = await _userAdminServices.UpdateUser(user);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error updating user: {ex.Message}");
                }
            }

            [HttpDelete]
            [Route("deleteUser")]
            public async Task<IActionResult> DeleteUser(string emailAddress)
            {
                try
                {
                    var result = await _userAdminServices.DeleteUser(emailAddress);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error deleting user: {ex.Message}");
                }
            }
    }
}
