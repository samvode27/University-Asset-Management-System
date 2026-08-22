using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Roles;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class RoleRepository
    : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Role By Name
    // ================================================================

    public virtual async Task<Role?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                role => role.Name == name,
                cancellationToken);
    }


    // ================================================================
    // Get Active Roles
    // ================================================================

    public virtual async Task<IReadOnlyList<Role>> GetActiveRolesAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(role => role.IsActive)
            .OrderBy(role => role.Name)
            .ToListAsync(cancellationToken);
    }



    public virtual async Task<Role?> GetByIdWithDetailsAsync(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Role ID is required.",
                nameof(id));
        }

        return await DbSet
            .Include(role => role.RolePermissions)
                .ThenInclude(rolePermission => rolePermission.Permission)
            .FirstOrDefaultAsync(
                role => role.Id == id,
                cancellationToken);
    }


    // ================================================================
    // Get System Roles
    // ================================================================

    public virtual async Task<IReadOnlyList<Role>> GetSystemRolesAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(role => role.IsSystemRole)
            .OrderBy(role => role.Name)
            .ToListAsync(cancellationToken);
    }
}