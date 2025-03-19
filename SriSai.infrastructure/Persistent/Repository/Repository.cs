using Microsoft.EntityFrameworkCore;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Base;
using SriSai.infrastructure.Persistent.DbContext;
using System.Linq.Expressions;

namespace SriSai.Infrastructure.Persistent.Repository
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly SriSaiDbContext _context;

        public Repository(SriSaiDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<T?> FindOneWithIncludeAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.Where(predicate).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindAllWithIncludeAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllForConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAllForConditionWithIncludeAsync(
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ToListAsync();
        }
    }
}