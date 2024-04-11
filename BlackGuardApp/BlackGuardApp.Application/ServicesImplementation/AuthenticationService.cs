using BlackGuardApp.Application.DTOs.AuthenticationDTO;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Domain;
using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
                            JWToken = GenerateJwtToken(existingUser, role),
                            IsPasswordSet = true,
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
                _logger.LogError($"An error occurred while trying to login: {ex}");
                return ApiResponse<LoginResponseDto>.Failed(false, "An error occurred while trying to login." + ex.Message, 
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

        public async Task<ApiResponse<SetPassRespDto>> SetPasswordAsync(string email, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    return ApiResponse<SetPassRespDto>.Failed(false, "Passwords do not match.", StatusCodes.Status400BadRequest, new List<string>());
                }

                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    return ApiResponse<SetPassRespDto>.Failed(false, "User not found.", StatusCodes.Status404NotFound, new List<string>());
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

                var result = await _userManager.ResetPasswordAsync(existingUser, token, password);
                if (result.Succeeded)
                {
                    existingUser.IsPasswordSet = true;
                    await _userManager.UpdateAsync(existingUser);
                    var response = new SetPassRespDto
                    {
                        IsPasswordSet = true,
                    };
                    return ApiResponse<SetPassRespDto>.Success(response, "Password set successfully, you can proceed to login", StatusCodes.Status200OK);
                }
                else
                {
                    return ApiResponse<SetPassRespDto>.Failed(false, "Failed to set password.", StatusCodes.Status500InternalServerError, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while setting password: {ex}");
                return ApiResponse<SetPassRespDto>.Failed(false, "An error occurred while setting password: " + ex.Message,
                    StatusCodes.Status500InternalServerError, new List<string>() { ex.Message });
            }
        }

        public ApiResponse<string> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:Secret").Value);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config.GetSection("JwtSettings:ValidIssuer").Value,
                    ValidAudience = _config.GetSection("JwtSettings:ValidAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                var emailClaim = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

                return new ApiResponse<string>(true, "Token is valid.", StatusCodes.Status200OK, new List<string>());
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError($"An error occurred while trying to validate token: {ex}");
                return new ApiResponse<string>(false, "Token validation failed.", StatusCodes.Status400BadRequest, 
                    new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to validate token: {ex}");
                return new ApiResponse<string>(false, "an error occurred during token validation", StatusCodes.Status500InternalServerError, 
                    new List<string>());
            }
        }
    }
}
