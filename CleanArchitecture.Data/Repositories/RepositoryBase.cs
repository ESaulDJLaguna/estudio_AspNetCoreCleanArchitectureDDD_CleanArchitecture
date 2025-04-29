using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.Repositories
{
	public class RepositoryBase<T> : IAsyncRepository<T> where T : BaseDomainModel
	{
		protected readonly StreamerDbContext _context;

		public RepositoryBase(StreamerDbContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicated)
		{
			return await _context.Set<T>().Where(predicated).ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(
			Expression<Func<T, bool>> predicated = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeString = null, bool disableTracking = true)
		{
			IQueryable<T> query = _context.Set<T>();

			if(disableTracking) query = query.AsNoTracking();

			if(!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

			if(predicated is not null) query = query.Where(predicated);

			if (orderBy is not null) return await orderBy(query).ToListAsync();

			return await query.ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(
			Expression<Func<T, bool>> predicated = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			List<Expression<Func<T, object>>> includes = null,
			bool disableTracking = true)
		{
			IQueryable<T> query = _context.Set<T>();

			if (disableTracking) query = query.AsNoTracking();

			if(includes is not null) query = includes.Aggregate(query, (current, include) => current.Include(include));

			if (predicated is not null) query = query.Where(predicated);

			if(orderBy is not null) return await orderBy(query).ToArrayAsync();

			return await query.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public async Task<T> AddAsync(T entity)
		{
			_context.Set<T>().Add(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<T> UpdateAsync(T entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task DeleteAsync(T entity)
		{
			_context.Set<T>().Remove(entity);
			await _context.SaveChangesAsync();
		}
	}
}
