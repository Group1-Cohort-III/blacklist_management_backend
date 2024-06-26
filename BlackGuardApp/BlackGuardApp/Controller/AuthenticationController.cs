﻿using BlackGuardApp.Application.DTOs.AuthenticationDTO;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlackGuardApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthenticationController(IAuthenticationService authService, SignInManager<AppUser> signInManager)
        {
            _authService = authService;
            _signInManager = signInManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            return Ok(await _authService.LoginAsync(loginRequest));
        }

        [HttpPost("Set-Password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto setPasswordDto)
        {
            return Ok(await _authService.SetPasswordAsync(setPasswordDto.Email, setPasswordDto.Password, setPasswordDto.ConfirmPassword));
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
