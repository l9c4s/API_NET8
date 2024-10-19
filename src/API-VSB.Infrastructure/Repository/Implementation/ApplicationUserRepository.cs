
using API.Domain.Entities;
using API.Infrastructure.Data;
using API.Infrastructure.Repository.Interface;

namespace API.Infrastructure.Repository.Implementation
{
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
	{

		private readonly ApplicationDbContext _db;
		public ApplicationUserRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

	}
}
