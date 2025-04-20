using System.ComponentModel.DataAnnotations;

namespace API.Domain.Dto
{
	public class RegisterModel
	{

		[Display(Name = "User@")]
		[Required(ErrorMessage = "O campo User@ é obrigatório.")]
		public required string UserName { get; set; }

		[Display(Name = "Nome")]
		[Required(ErrorMessage = "O campo Nome é obrigatório.")]
		public required string NomeCompleto { get; set; }

		[EmailAddress]
		[Required(ErrorMessage = "O campo Email é obrigatório.")]
		public required string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "O campo Senha é obrigatório.")]
		public required string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
		public required string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "O campo Role é obrigatório.")]
		public required string Role { get; set; }
	}
}
