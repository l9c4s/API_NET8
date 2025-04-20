using API.Application.Services.Interface;
using API.Domain.Dto;
using API.Domain.Entities;
using API_VSB.Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace API.Application.Services.Implementation
{
	public class AccountService : IAccountServices
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IMapper _mapper;
		private readonly ITokenService _tokenService;
		private readonly RoleManager<IdentityRole> _roleManager;


		public AccountService
		 (
		 UserManager<ApplicationUser> userManager,
		 IMapper mapper,
		 IConfiguration configuration,
		 RoleManager<IdentityRole> roleManager,
		 SignInManager<ApplicationUser> signInManager,
		 ITokenService tokenService
		 )
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_tokenService = tokenService;
			_mapper = mapper;
			_signInManager = signInManager;
		}


		public async Task<Result> Login(LoginModel model)
		{
			Result result = new();
			ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);

			if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				var roles = await _userManager.GetRolesAsync(user);
				var accessToken = _tokenService.GenerateAccessToken(user, roles);
				var refreshToken = _tokenService.GenerateRefreshToken();

				// Obter os acessos do token
				var claimsPrincipal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
				var accesses = claimsPrincipal?.Claims
					.Where(c => c.Type == "Access")
					.Select(c => c.Value)
					.ToList();


				// Salvar o Refresh Token na tabela AspNetUserTokens
				var tokenEntry = await _userManager.GetAuthenticationTokenAsync(user, "RefreshToken", "RefreshToken");
				if (tokenEntry != null)
				{
					await _userManager.RemoveAuthenticationTokenAsync(user, "RefreshToken", "RefreshToken");
				}
				await _userManager.SetAuthenticationTokenAsync(user, "RefreshToken", "RefreshToken", refreshToken);

				result.Response = new
				{
					Token = accessToken,
					Roles = roles,
					RefreshToken = refreshToken
				};
				result.Code = 200;
			}
			else
			{
				result.Code = 400;
				result.Error = "Email or Password is incorrect!";
			}

			return result;
		}

		public async Task<Result> Logout()
		{
			await _signInManager.SignOutAsync();
			return new Result { Code = 200 };
		}

		public async Task<Result> Register(RegisterModel model)
		{
			Result result = new();


			if (await _userManager.FindByEmailAsync(model.Email) is not null)
			{
				result.Code = 400;
				result.Error = "The email is already in use.";
				return result;
			}

			if (await _userManager.Users.AnyAsync(u => u.UserName == model.NomeCompleto))
			{
				result.Code = 400;
				result.Error = "The username is already in use.";
				return result;
			}

			// Verificar se a role fornecida existe
			if (!await _roleManager.RoleExistsAsync(model.Role))
			{
				result.Code = 400;
				result.Error = $"A role '{model.Role}' não existe.";
				return result;
			}


			ApplicationUser user = _mapper.Map<ApplicationUser>(model);

			IdentityResult createUserResult = await _userManager.CreateAsync(user, model.Password);

			if (createUserResult.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, model.Role);

				var roles = await _userManager.GetRolesAsync(user);
				var accessToken = _tokenService.GenerateAccessToken(user, roles);
				var refreshToken = _tokenService.GenerateRefreshToken();

				// Salvar o Refresh Token na tabela AspNetUserTokens
				await _userManager.SetAuthenticationTokenAsync(user, "RefreshToken", "RefreshToken", refreshToken);

				result.Response = new
				{
					Token = accessToken,
					Roles = roles,
					RefreshToken = refreshToken
				};
				result.Code = 200;
			}
			else
			{
				result.Code = 400;
				result.Error = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
			}
			return result;
		}

		public async Task<Result> RemoverUsuario(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			await _userManager.DeleteAsync(user);

			Result result = new();
			result.Code = 200;

			return result;
		}

		public async Task<Result> EnsureRolesExist()
		{
			var roles = new[] { "Admin", "Gerente", "Usuario" };
			foreach (var role in roles)
			{
				if (!await _roleManager.RoleExistsAsync(role))
				{
					var result = await _roleManager.CreateAsync(new IdentityRole(role));
					if (!result.Succeeded)
					{
						return new Result
						{
							Code = 500,
							Error = $"Failed to create role: {role}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}"
						};
					}
				}
			}
			return new Result { Code = 200, Response = "Roles created successfully." };
		}

		public async Task<Result> ConfigureRoleClaims()
		{
			var adminRole = await _roleManager.FindByNameAsync("Admin");
			if (adminRole != null)
			{
				await _roleManager.AddClaimAsync(adminRole, new Claim("Access", "ListarUsuarios"));
				await _roleManager.AddClaimAsync(adminRole, new Claim("Access", "ListaDeGaragem"));
			}

			var gerenteRole = await _roleManager.FindByNameAsync("Gerente");
			if (gerenteRole != null)
			{
				await _roleManager.AddClaimAsync(gerenteRole, new Claim("Access", "ListaDeGaragem"));
			}

			var usuarioRole = await _roleManager.FindByNameAsync("Usuario");
			if (usuarioRole != null)
			{
				await _roleManager.AddClaimAsync(usuarioRole, new Claim("Access", "ListaDeGaragem"));
			}

			return new Result { Code = 200, Response = "Claims configuradas com sucesso." };
		}


	}



}
