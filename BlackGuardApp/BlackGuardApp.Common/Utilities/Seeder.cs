using BlackGuardApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BlackGuardApp.Common.Utilities
{
    public class Seeder
    {
        public static async Task SeedRolesAndUserAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var seededAdmin = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!await roleManager.RoleExistsAsync("UserAdmin"))
            {
                var userAdminRole = new IdentityRole("UserAdmin");
                await roleManager.CreateAsync(userAdminRole);
            }

            if (!await roleManager.RoleExistsAsync("BlackListAdmin"))
            {
                var blackListAdminRole = new IdentityRole("BlackListAdmin");
                await roleManager.CreateAsync(blackListAdminRole);
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                var userRole = new IdentityRole("User");
                await roleManager.CreateAsync(userRole);
            }

            if (await seededAdmin.FindByEmailAsync("useradmin@blackgaurd.com") == null)
            {
                var userAdmin = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "UserAdmin",
                    LastName = "UserAdmin",
                    UserName = "useradmin@blackgaurd.com",
                    Email = "useradmin@blackgaurd.com",
                    NormalizedEmail = "useradmin@blackgaurd.com".ToUpper(),
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "1234567890",
                    CreatedAt = DateTime.UtcNow.Date,
                };

                var result = await seededAdmin.CreateAsync(userAdmin, "UserAdmin123@");
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create UserAdmin.");
                }
                else
                {
                    await seededAdmin.AddToRoleAsync(userAdmin, "UserAdmin");
                }
            }
        }
    }
}
