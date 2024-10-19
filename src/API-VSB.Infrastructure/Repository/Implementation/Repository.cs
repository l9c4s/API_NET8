using API.Infrastructure.Data;
using API.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Infrastructure.Repository.Implementation
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _applicationDbContext;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext db)
		{
			_applicationDbContext = db;
			dbSet = _applicationDbContext.Set<T>();
		}

		public async Task<T> Add(T entity)
		{
			var entidade = await dbSet.AddAsync(entity);
			return entidade.Entity;
		}

		public async Task<bool> Any(Expression<Func<T, bool>> filter)
		{
			return await dbSet.AnyAsync(filter);
		}

		public async Task<T?> Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T>? query = tracked ? dbSet.Where(filter) : dbSet.AsNoTracking().Where(filter);

			if (!string.IsNullOrEmpty(includeProperties) && query is not null)
			{
				//Farm,FarmNumber -- case sensitive
				foreach (var includeProp in includeProperties
					.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp.Trim());
				}
			}
			return await query?.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

			// Apply filtering if a filter is provided
			if (filter != null)
			{
				query = query.Where(filter);
			}

			// Apply include properties for navigation properties
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp.Trim());
				}
			}

			// Return the list of entities (query will not be null)
			return await query.ToListAsync();
		}

		public async Task Remove(T Entity)
		{
			dbSet.Remove(Entity);
		}
	}
}
