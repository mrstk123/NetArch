#if (IsEFCore || IsHybrid)
#if (IsClean)
using NetArch.Template.Domain.Interfaces;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Interfaces;
#endif
using NetArch.Template.Infrastructure.Persistence.Context;

namespace NetArch.Template.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private bool _disposed;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
            _context.Dispose();
        _disposed = true;
    }
}
#elif (IsDapper)
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
#if (IsClean)
using NetArch.Template.Domain.Interfaces;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Interfaces;
#endif

namespace NetArch.Template.Infrastructure.Persistence.Repositories;

/// <summary>
/// Dapper UnitOfWork — wraps an IDbTransaction so multiple repository operations
/// can participate in a single atomic operation.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly string _connectionString;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

    public UnitOfWork(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection Connection => _connection ??= new SqlConnection(_connectionString);

    public IDbTransaction BeginTransaction()
    {
        if (_connection?.State != ConnectionState.Open)
        {
            _connection ??= new SqlConnection(_connectionString);
            _connection.Open();
        }
        _transaction = _connection.BeginTransaction();
        return _transaction;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        _transaction?.Commit();
        return await Task.FromResult(0);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
        _disposed = true;
    }
}
#endif
