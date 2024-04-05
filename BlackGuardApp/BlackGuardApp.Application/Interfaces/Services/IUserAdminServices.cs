using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Domain.Entities;

namespace BlackGuardApp.Application.Interfaces.Services
{
    public interface IUserAdminServices
    {
        Task<string> CreateUser(string emailAddress);
        Task<AppUser> GetUserByEmail(string emailAddress);
        Task<string> UpdateUser(AppUserDtos user);
        Task<string> DeleteUser(string emailAddress);
    }
}
