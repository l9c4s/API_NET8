using API.Domain.Dto;
using API.Domain.Entities;


namespace API.Application.Services.Interface
{
	public interface IAccountServices
	{
		public Task<Result> Register(RegisterModel model);
		public Task<Result> Login(LoginModel model);
		public Task<Result> Logout();
		public Task<Result> RemoverUsuario(string id);

	}
}
