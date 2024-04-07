using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Application.ServicesImplementation;
using BlackGuardApp.Persistence.AppContext;
using BlackGuardApp.Persistence.Repositories;
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

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBlacklistService, BlacklistService>();
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
