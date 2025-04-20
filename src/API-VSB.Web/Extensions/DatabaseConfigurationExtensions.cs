using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public static class DatabaseConfigurationExtensions
{
	public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
			dbContextOptions
				.UseMySql(connectionString, new MySqlServerVersion(new Version(11, 3, 2)))
				.LogTo(Console.WriteLine, LogLevel.Information)
				.EnableSensitiveDataLogging()
				.EnableDetailedErrors()
		);

		return services;
	}
}