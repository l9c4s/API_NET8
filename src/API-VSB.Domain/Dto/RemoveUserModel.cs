
using System.ComponentModel.DataAnnotations;

namespace API_VSB.Domain.Dto
{
	public class RemoveUserModel
	{
		[Required(ErrorMessage = "O campo IdUser é obrigatório.")]
		public int IdUser { get; set; }

	}
}
