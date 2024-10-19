using API.Application.Services.Interface;
using API.Domain.Dto;
using API.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;


		public AccountService
		 (
		 UserManager<ApplicationUser> userManager,
		 RoleManager<IdentityRole> roleManager,
		 IMapper mapper,
		 IConfiguration configuration,
		  SignInManager<ApplicationUser> signInManager
		 )
		{
			_configuration = configuration;
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
			_signInManager = signInManager;
		}


		public async Task<Result> Login(LoginModel model)
		{
			Result result = new();
			ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);
			if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				result.Response = GetToken(user);
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
				result.Error = "The User is already exist";
			}
			else
			{
				ApplicationUser MapUser = _mapper.Map<ApplicationUser>(model);
				var CreateUser = await _userManager.CreateAsync(MapUser, model.Password);

				if (CreateUser.Succeeded)
				{
					result.Response = GetToken(MapUser);
					result.Code = 200;
				}
				else
				{
					result.Code = 400;

					var errors = string.Empty;

					foreach (var error in CreateUser.Errors)
						errors += $"{error.Description},";
					result.Error = errors;
				}
			}


			return result;
		}

		public async Task<Result> RemoverUsuario(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
		    await _userManager.DeleteAsync(user);

			Result result = new();
			result.Code = 200;
			var errors = string.Empty;

			return result;
		}



		private string GetToken(ApplicationUser applicationUser)
		{
			var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"] ?? string.Empty));
			var isuer = _configuration["JWT:Issuer"];
			var audience = _configuration["JWT:Audience"];
			var credenctial = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);



			var tokenOptions = new JwtSecurityToken(
				issuer: isuer,
				audience: audience,
				claims: new[]
				{
						new Claim(type: ClaimTypes.Name, applicationUser.UserName)
				},
				expires: DateTime.Now.AddHours(2),
				signingCredentials: credenctial
				);

			return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		}
	}
}
