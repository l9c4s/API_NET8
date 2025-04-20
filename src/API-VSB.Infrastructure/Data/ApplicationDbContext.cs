using API.Domain.Entities;
using API_VSB.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


		public DbSet<SocialMedia> SocialMedias { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Configuração para SocialMedia
			builder.Entity<SocialMedia>()
				.HasOne(sm => sm.User)
				.WithMany(u => u.SocialMedias)
				.HasForeignKey(sm => sm.UserId)
				.OnDelete(DeleteBehavior.Cascade);


			// Configuração para Username único
			builder.Entity<ApplicationUser>()
				.HasIndex(u => u.UserName)
				.IsUnique();

		}
	}
}
