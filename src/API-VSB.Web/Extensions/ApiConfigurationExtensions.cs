using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

public static class ApiConfigurationExtensions
{
	public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
	{
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo()
			{
				Version = "v1",
				Title = "API.VSB",
				Description = "Apis para sistema mobile"
			});

			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "Cabeçalho de Autorização JWT usando o esquema Bearer"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				Array.Empty<string>()
			}});
		});

		return services;
	}
}
