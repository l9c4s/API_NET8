using System.ComponentModel.DataAnnotations;

namespace API.Domain.Dto
{
	public class RegisterModel
	{
		[Display(Name = "Nome_Companhia")]
		public required string UserName { get; set; }

		[EmailAddress]
		public required string Email { get; set; }

		[DataType(DataType.Password)]
		public required string Password { get; set; }
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

		public required string CNPJ { get; set; }

	}
}
