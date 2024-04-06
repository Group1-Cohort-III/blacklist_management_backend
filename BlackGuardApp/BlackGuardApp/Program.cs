using BlackGuardApp.APIConfigurations;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Mapper;
using BlackGuardApp.Persistence.ServiceExtension;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
	var builder = WebApplication.CreateBuilder(args);

	ConfigurationHelper.InstantiateConfiguration(builder.Configuration);
	var configuration = builder.Configuration;


	builder.Services.AddControllers();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();
	builder.Services.AddSwagger();
	builder.Services.AddDependencies(configuration);
    builder.Services.ConfigureAuthentication(configuration);
    builder.Services.AddAutoMapper(typeof(MapperProfile));
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        await Seeder.SeedRolesAndUserAdmin(serviceProvider);
    }

    app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Something is not right here");
}
finally
{
    NLog.LogManager.Shutdown();
}