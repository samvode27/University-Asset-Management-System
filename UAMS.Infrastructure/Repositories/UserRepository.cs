using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Users;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class UserRepository
    : GenericRepository<User>, IUserRepository
{
    public UserRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get User By Username
    // ================================================================

    public virtual async Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                user => user.Username == username,
                cancellationToken);
    }

    // ================================================================
    // Get User By Username For Authentication
    // ================================================================

    public virtual async Task<User?> GetByUsernameForAuthenticationAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return await DbSet
            .FirstOrDefaultAsync(
                user => user.Username == username,
                cancellationToken);
    }


    // ================================================================
    // Get User By Email
    // ================================================================

    public virtual async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken);
    }


    // ================================================================
    // Authentication - Get User By Email
    // ================================================================

    public virtual async Task<User?> GetByEmailForAuthenticationAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return await DbSet
            .FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken);
    }


    // ================================================================
    // Get User By Employee ID
    // ================================================================

    public virtual async Task<User?> GetByEmployeeIdAsync(
        string employeeId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(employeeId);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                user => user.EmployeeId == employeeId,
                cancellationToken);
    }


    // ================================================================
    // Get User By Id With Authentication / Authorization Data
    // ================================================================

    public virtual async Task<User?> GetByIdWithAuthenticationDataAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(user => user.Department)

            .Include(user => user.UserRoles
                .Where(userRole =>
                    userRole.IsActive))

                .ThenInclude(userRole => userRole.Role)

                    .ThenInclude(role => role.RolePermissions
                        .Where(rolePermission =>
                            rolePermission.IsActive))

                        .ThenInclude(rolePermission =>
                            rolePermission.Permission)

            .FirstOrDefaultAsync(
                user => user.Id == id,
                cancellationToken);
    }

    // ================================================================
    // Exists By Username
    // ================================================================

    public virtual async Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return await DbSet
            .AnyAsync(
                user => user.Username == username,
                cancellationToken);
    }


    // ================================================================
    // Exists By Email
    // ================================================================

    public virtual async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return await DbSet
            .AnyAsync(
                user => user.Email == email,
                cancellationToken);
    }


    // ================================================================
    // Exists By Employee ID
    // ================================================================

    public virtual async Task<bool> ExistsByEmployeeIdAsync(
        string employeeId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(employeeId);

        return await DbSet
            .AnyAsync(
                user => user.EmployeeId == employeeId,
                cancellationToken);
    }

    // ================================================================
    // Get Active Users
    // ================================================================

    public virtual async Task<IReadOnlyList<User>> GetActiveUsersAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(user => user.IsActive)
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Inactive Users
    // ================================================================

    public virtual async Task<IReadOnlyList<User>> GetInactiveUsersAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(user => !user.IsActive)
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Users By Department
    // ================================================================

    public virtual async Task<IReadOnlyList<User>> GetByDepartmentAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(user => user.DepartmentId == departmentId)
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Users By Role
    // ================================================================

    public virtual async Task<IReadOnlyList<User>> GetByRoleAsync(
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(user => user.UserRoles
                .Any(userRole => userRole.RoleId == roleId))
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }
}