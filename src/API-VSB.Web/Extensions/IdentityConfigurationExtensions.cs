using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

public static class IdentityConfigurationExtensions
{
	public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
	{
		services.AddIdentity<ApplicationUser, IdentityRole>(options =>
		{
			options.User.RequireUniqueEmail = true;
			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireUppercase = true;
			options.Password.RequireNonAlphanumeric = true;
			options.Password.RequiredLength = 12;
			options.SignIn.RequireConfirmedEmail = false;
		})
		.AddEntityFrameworkStores<ApplicationDbContext>();

		return services;
	}
}