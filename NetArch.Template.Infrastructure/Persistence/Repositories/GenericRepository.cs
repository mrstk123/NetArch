#if (IsEFCore)
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
#if (IsClean)
using NetArch.Template.Domain.Interfaces;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Interfaces;
#endif
using NetArch.Template.Infrastructure.Persistence.Context;

namespace NetArch.Template.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;
        foreach (var include in includes)
            query = query.Include(include);
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);
        foreach (var include in includes)
            query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);
        foreach (var include in includes)
            query = query.Include(include);
        return await orderBy(query).ToListAsync();
    }

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity);
}
#elif (IsHybrid)
using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
#if (IsClean)
using NetArch.Template.Domain.Interfaces;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Interfaces;
#endif
using NetArch.Template.Infrastructure.Persistence.Context;

namespace NetArch.Template.Infrastructure.Persistence.Repositories;

/// <summary>
/// Hybrid repository: EF Core for writes, Dapper for reads.
/// </summary>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly string _connectionString;

    public GenericRepository(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    // --- Reads via Dapper ---

    public async Task<T?> GetByIdAsync(int id)
    {
        var tableName = typeof(T).Name;
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<T>(
            $"SELECT * FROM [{tableName}s] WHERE Id = @Id", new { Id = id });
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        // For complex queries with includes, fall back to EF
        IQueryable<T> query = _dbSet.AsNoTracking();
        foreach (var include in includes)
            query = query.Include(include);
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var tableName = typeof(T).Name;
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<T>($"SELECT * FROM [{tableName}s]");
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);
        foreach (var include in includes)
            query = query.Include(include);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);
        foreach (var include in includes)
            query = query.Include(include);
        return await orderBy(query).ToListAsync();
    }

    // --- Writes via EF Core ---

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity);
}
#elif (IsDapper)
using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
#if (IsClean)
using NetArch.Template.Domain.Interfaces;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Interfaces;
#endif

namespace NetArch.Template.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly string _connectionString;

    public GenericRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<T?> GetByIdAsync(int id)
    {
        var tableName = typeof(T).Name;
        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(
            $"SELECT * FROM [{tableName}s] WHERE Id = @Id", new { Id = id });
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        // Dapper does not support expression-based includes; delegate to simple GetById
        => await GetByIdAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var tableName = typeof(T).Name;
        using var connection = CreateConnection();
        return await connection.QueryAsync<T>($"SELECT * FROM [{tableName}s]");
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => throw new NotSupportedException(
            "Expression-based filtering is not supported with Dapper. Use a raw SQL overload or extend the repository.");

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
        => await FindAsync(predicate);

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        params Expression<Func<T, object>>[] includes)
        => await FindAsync(predicate);

    public async Task AddAsync(T entity)
    {
        var tableName = typeof(T).Name;
        using var connection = CreateConnection();
        // Use Dapper.Contrib or write INSERT manually; stub shown here
        await connection.ExecuteAsync($"/* INSERT INTO [{tableName}s] ... */", entity);
    }

    public void Update(T entity)
    {
        // Synchronous stub — wrap in a transaction from UnitOfWork as needed
    }

    public void Delete(T entity)
    {
        // Synchronous stub — wrap in a transaction from UnitOfWork as needed
    }
}
#endif
