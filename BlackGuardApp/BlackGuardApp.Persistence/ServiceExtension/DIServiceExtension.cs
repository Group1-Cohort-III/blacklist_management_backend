using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Application.ServicesImplementation;
using BlackGuardApp.Domain.Entities;
using BlackGuardApp.Persistence.AppContext;
using BlackGuardApp.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlackGuardApp.Persistence.ServiceExtension
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BlackGADbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<BlackGADbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBlacklistService, BlacklistService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserAdminServices, UserAdminServices>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserAdminPolicy", policy =>
                {
                    policy.RequireRole("UserAdmin");
                    policy.RequireAuthenticatedUser();
                });
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}
