using SriSai.Domain.Entity.Base;
using System.Linq.Expressions;

namespace SriSai.Application.interfaces.Reposerty
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> ListAllAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}