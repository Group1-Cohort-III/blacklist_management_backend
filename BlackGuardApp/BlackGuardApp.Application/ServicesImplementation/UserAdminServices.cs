using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlackGuardApp.Application.ServicesImplementation
{
    public class UserAdminServices : IUserAdminServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public UserAdminServices(UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<string> CreateUser(string emailAddress)
        {
            var uniqueEmail = await _userManager.FindByEmailAsync(emailAddress);
            if (uniqueEmail != null)
            {
                return $"User with {uniqueEmail} already exists";
            }
            var user = new AppUser { UserName = emailAddress, Email = emailAddress };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "RegularUser");
                return $"{user.Email} created successfully";
            }
            else
            {
                var error = result.Errors.FirstOrDefault()?.Description;
                throw new ApplicationException(error ?? "Unknown error occurred while creating user.");
            }
        }

        public async Task<AppUser> GetUserByEmail(string emailAddress)
        {
            var result = await _userManager.FindByEmailAsync(emailAddress);
            ArgumentNullException.ThrowIfNull(result);
            return result;
        }

        public async Task <List<AppUser>> GetAllUsers()
        {
            var users = await _unitOfWork.AppUserRepository.GetAllAsync();

            return users;
        }

        public async Task<string> UpdateUser(AppUserDtos user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser == null)
            {
                return $"{existingUser} not found";
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                return $"failed to update {existingUser}";
            }

            return $"{existingUser} updated successfully";
        }

        public async Task<string> DeleteUser(string emailAddress)
        {
            var result = await _userManager.FindByEmailAsync(emailAddress);
            ArgumentNullException.ThrowIfNull(result);
            await _userManager.DeleteAsync(result);
            var deleted = await _unitOfWork.SaveChangesAsync();
            if (deleted > 0)
            {
                return $"{result.FirstName} deleted successfuly";
            }
            return $"Error occurred while deleting {result.FirstName}";
        }
    }
}
