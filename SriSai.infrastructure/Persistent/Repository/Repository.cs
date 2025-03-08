using Microsoft.EntityFrameworkCore;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Base;
using SriSai.infrastructure.Persistent.DbContext;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


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
            => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> ListAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().Where(predicate).ToListAsync();

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
    }
}