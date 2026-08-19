using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities;
using UAMS.Domain.Entities.Permissions;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class PermissionRepository
    : GenericRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Permission By Name
    // ================================================================

    public virtual async Task<Permission?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                permission => permission.Name == name,
                cancellationToken);
    }


    // ================================================================
    // Get Permissions By Module
    // ================================================================

    public virtual async Task<IReadOnlyList<Permission>> GetByModuleAsync(
        string module,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(module);

        return await DbSet
            .AsNoTracking()
            .Where(permission => permission.Module == module)
            .OrderBy(permission => permission.Name)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Active Permissions
    // ================================================================

    public virtual async Task<IReadOnlyList<Permission>> GetActivePermissionsAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(permission => permission.IsActive)
            .OrderBy(permission => permission.Module)
            .ThenBy(permission => permission.Name)
            .ToListAsync(cancellationToken);
    }
}