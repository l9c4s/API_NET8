using API.Application.Services.Interface;
using API.Domain.Dto;
using API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Web.Controllers
{
	[Controller]
	[Route("Api/[Controller]")]
	public class AccountController(IAccountServices _accountServices) : ControllerBase
	{
		[Authorize]
		[HttpPost("Deleteauth")]
		public async Task<IActionResult> Deletar1([FromBody] string ididel)
		{

			var result = await _accountServices.RemoverUsuario(ididel);
			return Ok(result);
		}

		[HttpPost("Delete")]
		public async Task<IActionResult> Deletar([FromBody] string ididel)
		{

			var result = await _accountServices.RemoverUsuario(ididel);
			return Ok(result);
		}


		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
		{

			var result = await _accountServices.Register(registerModel);
			return Ok(result);

		}


		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
		{
			if (!ModelState.IsValid)
			{
				return Ok(new Result { Code = 400, Error = "The Model Is Invalid" });

			}
			var result = await _accountServices.Login(loginModel);

			if(result.Code == 200)
			{

			}

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
