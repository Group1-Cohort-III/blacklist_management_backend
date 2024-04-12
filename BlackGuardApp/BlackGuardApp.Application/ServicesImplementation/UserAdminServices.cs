using AutoMapper;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain;
using BlackGuardApp.Domain.Entities;
using BlackGuardApp.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BlackGuardApp.Application.ServicesImplementation
{
    public class UserAdminServices : IUserAdminServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAdminServices> _logger;
        public UserAdminServices(UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork, ILogger<UserAdminServices> logger, 
            RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateUserAsync(string emailAddress, UserRoles[] roles)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(emailAddress);
                if (existingUser != null)
                {
                    return ApiResponse<string>.Failed(false, $"User with email address '{emailAddress}' already exists.",
                                                       StatusCodes.Status400BadRequest, new List<string>());
                }

                var user = new AppUser
                {
                    UserName = emailAddress,
                    Email = emailAddress,
                    NormalizedEmail = emailAddress.ToUpper(),
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    var roleNames = roles.Select(role => role.ToString()).ToArray();

                    foreach (var role in roleNames)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            return ApiResponse<string>.Failed(false, $"Role '{role}' does not exist.",
                                                               StatusCodes.Status400BadRequest, new List<string>());
                        }
                    }

                    var assignRoleResult = await _userManager.AddToRolesAsync(user, roleNames);
                    if (assignRoleResult.Succeeded)
                    {
                        await _unitOfWork.SaveChangesAsync();
                        return ApiResponse<string>.Success($"User '{emailAddress}' created successfully", "User created successfully",
                                                           StatusCodes.Status201Created);
                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                        return ApiResponse<string>.Failed(false, "Failed to assign roles to the user.",
                                                           StatusCodes.Status500InternalServerError, new List<string>());
                    }
                }
                else
                {
                    return ApiResponse<string>.Failed(false, "Unknown error occurred while creating user.",
                                                       StatusCodes.Status500InternalServerError, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user.");
                return ApiResponse<string>.Failed(false, "An error occurred while creating user.",
                                                   StatusCodes.Status500InternalServerError, new List<string>());
            }
        }


        public async Task<AppUser> GetUserByEmail(string emailAddress)
        {
            var result = await _userManager.FindByEmailAsync(emailAddress);
            ArgumentNullException.ThrowIfNull(result);
            return result;
        }

        public async Task<ApiResponse<PageResult<IEnumerable<GetAllUsersDto>>>> GetAllUsersAsync(int page, int perPage)
        {
            try
            {
                var allUsers = await _unitOfWork.AppUserRepository.GetAllAsync();
                var pagedUsers = await Pagination<AppUser>.GetPager(
                    allUsers,
                    perPage,
                    page,
                    user => user.LastName,
                    user => user.Id.ToString());
                var pagedUserDtos = _mapper.Map<PageResult<IEnumerable<GetAllUsersDto>>>(pagedUsers);

                return ApiResponse<PageResult<IEnumerable<GetAllUsersDto>>>.Success(pagedUserDtos, "Users found.", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users by pagination. Page: {Page}, PerPage: {PerPage}", page, perPage);
                return ApiResponse<PageResult<IEnumerable<GetAllUsersDto>>>.Failed(false, "An error occurred while retrieving users by pagination.", 500, new List<string> { ex.Message });
            }
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
            var deleted = await _userManager.DeleteAsync(result);
            await _unitOfWork.SaveChangesAsync();
            if (deleted.Succeeded)
            {
                return $"{result.FirstName} deleted successfuly";
            }
            return $"Error occurred while deleting {result.FirstName}";
        }
    }
}
