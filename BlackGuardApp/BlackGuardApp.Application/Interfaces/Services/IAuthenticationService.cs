using BlackGuardApp.Application.DTOs.AuthenticationDTO;
using BlackGuardApp.Domain;

namespace BlackGuardApp.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto);
        Task<ApiResponse<string>> SetPasswordAsync(string email, string password, string confirmPassword);
        Task<ApiResponse<string>> ValidateTokenAsync(string token);
    }
}
