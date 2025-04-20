using API.Application.Services.Implementation;
using API.Application.Services.Interface;
using API.Infrastructure.Repository.Implementation;
using API.Infrastructure.Repository.Interface;
using API.Infrastructure.UnitOfWork;
using API_VSB.Application.Services.Implementation;
using API_VSB.Application.Services.Interface;

namespace Riders_API.Extensions
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
		{
			// Registrar serviços e repositórios
			services.AddScoped<IAccountServices, AccountService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
			services.AddScoped<ITokenService, TokenService>();

			// Registrar AutoMapper
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			return services;
		}
	}
}
