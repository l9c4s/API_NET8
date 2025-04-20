using API.Domain.Entities;
using API_VSB.Application.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API_VSB.Application.Services.Implementation
{


	public class TokenService : ITokenService
	{

		private readonly IConfiguration _configuration;
		private readonly RoleManager<IdentityRole> _roleManager;

		public TokenService(IConfiguration configuration, RoleManager<IdentityRole> roleManager)
		{

			_configuration = configuration;
			_roleManager = roleManager;
		}


		public string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, user.Id)
			};

			// Adicionar roles ao token
			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


			// Obter as claims de acesso associadas às roles
			foreach (var role in roles)
			{
				var roleEntity = _roleManager.FindByNameAsync(role).Result;
				if (roleEntity != null)
				{
					var roleClaims = _roleManager.GetClaimsAsync(roleEntity).Result;
					claims.AddRange(roleClaims.Where(c => c.Type == "Access"));
				}
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:Issuer"],
				audience: _configuration["JWT:Audience"],
				claims: claims,
				expires: DateTime.Now.AddHours(2),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
			}
			return Convert.ToBase64String(randomNumber);
		}

		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"])),
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = _configuration["JWT:Issuer"],
				ValidAudience = _configuration["JWT:Audience"],
				ValidateLifetime = false // Ignorar a expiração do token
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}
	}
}
