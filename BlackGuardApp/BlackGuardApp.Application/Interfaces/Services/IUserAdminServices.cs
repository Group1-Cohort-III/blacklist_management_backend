using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain;
using BlackGuardApp.Domain.Entities;
using BlackGuardApp.Domain.Enum;

namespace BlackGuardApp.Application.Interfaces.Services
{
    public interface IUserAdminServices
    {
        Task<ApiResponse<string>> CreateUserAsync(string emailAddress, UserRoles[] roles);
        Task<ApiResponse<PageResult<IEnumerable<GetAllUsersDto>>>> GetAllUsersAsync(int page, int perPage);
        Task<AppUser> GetUserByEmail(string emailAddress);
        Task<string> UpdateUser(AppUserDtos user);
        Task<string> DeleteUser(string emailAddress);
    }
}
