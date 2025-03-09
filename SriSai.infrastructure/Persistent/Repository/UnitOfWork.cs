using Microsoft.EntityFrameworkCore.Storage;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Base;
using SriSai.infrastructure.Persistent.DbContext;

namespace SriSai.Infrastructure.Persistent.Repository;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly SriSaiDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(SriSaiDbContext context)
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : EntityBase
    {
        return new Repository<T>(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already active.");

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction.");

        await _transaction.CommitAsync();
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction.");

        await _transaction.RollbackAsync();
        _transaction.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}