using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly UAMSDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(UAMSDbContext context)
    {
        Context = context
            ?? throw new ArgumentNullException(nameof(context));

        DbSet = Context.Set<TEntity>();
    }


    // ================================================================
    // Query
    // ================================================================

    public virtual async Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(
            new object[] { id },
            cancellationToken);
    }


    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }


    public virtual async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }


    public virtual async Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbSet
            .AnyAsync(predicate, cancellationToken);
    }


    // ================================================================
    // Command
    // ================================================================

    public virtual async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await DbSet.AddAsync(
            entity,
            cancellationToken);
    }


    public virtual async Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await DbSet.AddRangeAsync(
            entities,
            cancellationToken);
    }


    public virtual void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbSet.Update(entity);
    }


    public virtual void UpdateRange(
        IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        DbSet.UpdateRange(entities);
    }


    public virtual void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbSet.Remove(entity);
    }


    public virtual void DeleteRange(
        IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        DbSet.RemoveRange(entities);
    }
}