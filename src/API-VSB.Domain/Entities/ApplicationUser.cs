using Microsoft.AspNetCore.Identity;

namespace API.Domain.Entities
{
	public class ApplicationUser : IdentityUser
	{
        public string CNPJ { get; set; }
    }
}
