using API.Domain.Entities;
using System.Security.Claims;

namespace API_VSB.Application.Services.Interface
{
	public interface ITokenService
	{
		string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles);
		string GenerateRefreshToken();
		ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
	}
}
