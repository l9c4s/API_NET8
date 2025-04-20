using API.Domain.Entities;
namespace API_VSB.Domain.Entities
{
	public class SocialMedia
	{
		public int Id { get; set; } // Chave primária

		// Nome da rede social (ex.: Instagram, Facebook, etc.)
		public string Platform { get; set; }

		// URL ou identificador do perfil na rede social
		public string ProfileUrl { get; set; }

		// Relacionamento com ApplicationUser
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
