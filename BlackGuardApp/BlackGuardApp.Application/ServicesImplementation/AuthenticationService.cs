using BlackGuardApp.Application.DTOs.AuthenticationDTO;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Domain;
using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlackGuardApp.Application.ServicesImplementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _config;

        public AuthenticationService(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            ILogger<AuthenticationService> logger, 
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);
                if (existingUser == null)
                {
                    return ApiResponse<LoginResponseDto>.Failed(false, "User not found.", StatusCodes.Status404NotFound, new List<string>());
                }
                if (!await _userManager.HasPasswordAsync(existingUser))
                {
                    return ApiResponse<LoginResponseDto>.Failed(false, "Password not set. Please set your password.", 
                        StatusCodes.Status400BadRequest, new List<string>());
                }

                var result = await _signInManager.CheckPasswordSignInAsync(existingUser, requestDto.Password, lockoutOnFailure: false);

                switch (result)
                {
                    case { Succeeded: true }:
                        var role = (await _userManager.GetRolesAsync(existingUser)).FirstOrDefault();
                        var response = new LoginResponseDto
                        {
                            JWToken = GenerateJwtToken(existingUser, role)
                        };
                        return ApiResponse<LoginResponseDto>.Success(response, "Logged In Successfully", StatusCodes.Status200OK);

                    case { IsLockedOut: true }:
                        return ApiResponse<LoginResponseDto>.Failed(false, $"Account is locked out. Please try again later or contact support." +
                            $" You can unlock your account after {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} minutes.", 
                            StatusCodes.Status403Forbidden, new List<string>());

                    case { RequiresTwoFactor: true }:
                        return ApiResponse<LoginResponseDto>.Failed(false, "Two-factor authentication is required.", 
                            StatusCodes.Status401Unauthorized, new List<string>());

                    case { IsNotAllowed: true }:
                        return ApiResponse<LoginResponseDto>.Failed(false, "Login failed. Email confirmation is required.", 
                            StatusCodes.Status401Unauthorized, new List<string>());

                    default:
                        return ApiResponse<LoginResponseDto>.Failed(false, "Login failed. Invalid email or password.", 
                            StatusCodes.Status401Unauthorized, new List<string>());
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.Failed(false, "Some error occurred while login in." + ex.Message, 
                    StatusCodes.Status500InternalServerError, new List<string>() { ex.Message });
            }
        }

        private string GenerateJwtToken(AppUser contact, string roles)
        {
            var jwtSettings = _config.GetSection("JwtSettings:Secret").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, contact.Id),
                new Claim(JwtRegisteredClaimNames.Email, contact.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, contact.FirstName+" "+contact.LastName),
                new Claim(ClaimTypes.Role, roles)
            };

            var token = new JwtSecurityToken(
                issuer: _config.GetValue<string>("JwtSettings:ValidIssuer"),
                audience: _config.GetValue<string>("JwtSettings:ValidAudience"),
                //issuer: null,
                //audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config.GetSection("JwtSettings:AccessTokenExpiration").Value)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
























        public Task<ApiResponse<string>> SetPasswordAsync(string email, string password, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> ValidateTokenAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
