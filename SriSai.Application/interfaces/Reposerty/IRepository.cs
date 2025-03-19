using SriSai.Domain.Entity.Base;
using System.Linq.Expressions;

namespace SriSai.Application.interfaces.Reposerty
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> FindAllForConditionAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> ListAllAsync();
        Task<IEnumerable<T>> ListAllForConditionWithIncludeAsync(params Expression<Func<T, object>>[] includes);

        Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate);

        Task<T?> FindOneWithIncludeAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);

        public Task<IEnumerable<T>> FindAllWithIncludeAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);

        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}