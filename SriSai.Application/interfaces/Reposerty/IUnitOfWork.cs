using SriSai.Domain.Entity.Base;

namespace SriSai.Application.interfaces.Reposerty;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : EntityBase;
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}