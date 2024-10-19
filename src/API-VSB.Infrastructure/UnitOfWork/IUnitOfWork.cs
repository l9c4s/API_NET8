using API.Infrastructure.Repository.Interface;

namespace API.Infrastructure.UnitOfWork
{
	public interface IUnitOfWork
	{

		IApplicationUserRepository ApplicationUser { get; set; }


		void SaveChanges();
		Task SaveChangesAsync();

	}
}
