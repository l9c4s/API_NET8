using API.Application.Services.Interface;
using API.Domain.Dto;
using API_VSB.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers
{
	[Controller]
	[Route("Api/[Controller]")]
	public class AccountController(IAccountServices _accountServices) : ControllerBase
	{


		[Authorize]
		[HttpPost("deleteauth")]
		public async Task<IActionResult> Deletar([FromBody] RemoveUserModel IdUser)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _accountServices.RemoverUsuario(IdUser.IdUser.ToString());
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
