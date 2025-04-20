using API.Application.Services.Interface;
using API.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Web.Controllers
{
	[Controller]
	[Route("Api/[Controller]")]
	public class AccountController(IAccountServices _accountServices) : ControllerBase
	{


		[Authorize]
		[HttpPost("deleteauth")]
		public async Task<IActionResult> Deletar1([FromBody] string ididel)
		{
			var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

			var result = await _accountServices.RemoverUsuario(ididel);
			return Ok(result);
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _accountServices.Register(registerModel);
			return Ok(result);

		}


		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _accountServices.Login(loginModel);
			return Ok(result);
		}

		[HttpPost("Logout")]
		public async Task<IActionResult> Logout()
		{
			var result = await _accountServices.Logout();
			return Ok(result);
		}
	}

}
