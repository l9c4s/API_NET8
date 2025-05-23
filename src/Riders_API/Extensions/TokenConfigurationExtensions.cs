﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Riders_API.Extensions
{
	public static class TokenConfigurationExtensions
	{
		public static IServiceCollection AddTokenConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(option =>
			{
				option.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					ValidateAudience = true,
					ValidAudience = configuration["JWT:Audience"],
					ValidateIssuer = true,
					ValidIssuer = configuration["JWT:Issuer"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"])),
				};
			});

			


			return services;
		}
	}
}
