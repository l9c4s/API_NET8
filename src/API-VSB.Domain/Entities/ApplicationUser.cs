using API_VSB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace API.Domain.Entities
{
	public class ApplicationUser : IdentityUser
	{

		public string NomeCompleto { get; set; }
		public ICollection<SocialMedia>? SocialMedias { get; set; } 
	}
}
