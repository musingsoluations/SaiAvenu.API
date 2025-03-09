using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Base;
using SriSai.infrastructure.Persistent.DbContext;

namespace SriSai.Infrastructure.Persistent.Repository;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    private readonly SriSaiDbContext _context;

    public Repository(SriSaiDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> ListAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
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
}