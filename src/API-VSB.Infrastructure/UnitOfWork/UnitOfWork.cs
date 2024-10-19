using API.Infrastructure.Data;
using API.Infrastructure.Repository.Implementation;
using API.Infrastructure.Repository.Interface;

namespace API.Infrastructure.UnitOfWork
{
	public class UnitOfWork(ApplicationDbContext applicationDbContext) : IUnitOfWork
	{
		public IApplicationUserRepository ApplicationUser { get; set; } = new ApplicationUserRepository(applicationDbContext);

		public void SaveChanges()
		{
			applicationDbContext.SaveChanges();
		}

		public async Task SaveChangesAsync()
		{
			await applicationDbContext.SaveChangesAsync();
		}
	}
}
