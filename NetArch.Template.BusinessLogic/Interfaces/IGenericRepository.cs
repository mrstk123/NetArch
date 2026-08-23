#if (IsEFCore)
using System.Linq.Expressions;
using NetArch.Template.BusinessLogic.Entities;

namespace NetArch.Template.BusinessLogic.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Fetches an entity by id as a tracked instance (suitable for read-modify-write flows).
    /// </summary>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetches an entity by id with related navigation properties, as a tracked instance.
    /// </summary>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Returns all entities as a read-only (no-tracking) snapshot.
    /// </summary>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns matching entities as a read-only (no-tracking) snapshot.
    /// </summary>
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    void Update(T entity);

    /// <summary>
    /// Soft-deletes the entity (<see cref="BaseEntity.IsActive"/> = false) on Save; queries filter inactive rows.
    /// </summary>
    void Delete(T entity);
}
#endif
