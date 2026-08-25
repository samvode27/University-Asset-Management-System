using Microsoft.EntityFrameworkCore;

using UAMS.Application.DTOs.Users;
using UAMS.Application.DTOs.Users.Requests;
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
    // Get User By Email For Authentication
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
    // Get User By ID With Details
    // ================================================================

    public virtual async Task<User?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(id));
        }

        return await DbSet
            .AsNoTracking()
            .Include(user => user.Department)
            .Include(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
            .FirstOrDefaultAsync(
                user =>
                    user.Id == id &&
                    !user.IsDeleted,
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
    // Get User By ID With Authentication / Authorization Data
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
    // Get Paged / Filtered Users
    // ================================================================

    public virtual async Task<UserQueryResult> GetPagedAsync(
        UserFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        IQueryable<User> query =
            DbSet
                .AsNoTracking()
                .Include(user => user.Department)
                .Include(user => user.UserRoles)
                    .ThenInclude(userRole => userRole.Role);


        // ============================================================
        // Search
        // ============================================================

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search =
                request.Search.Trim();

            query = query.Where(user =>
                user.EmployeeId.Contains(search) ||
                user.FullName.Contains(search) ||
                user.Email.Contains(search) ||
                user.Username.Contains(search));
        }


        // ============================================================
        // Department
        // ============================================================

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(user =>
                user.DepartmentId ==
                request.DepartmentId.Value);
        }


        // ============================================================
        // Role
        // ============================================================

        if (request.RoleId.HasValue)
        {
            query = query.Where(user =>
                user.UserRoles.Any(userRole =>
                    userRole.RoleId ==
                    request.RoleId.Value));
        }


        // ============================================================
        // Active Status
        // ============================================================

        if (request.IsActive.HasValue)
        {
            query = query.Where(user =>
                user.IsActive ==
                request.IsActive.Value);
        }


        // ============================================================
        // Locked Status
        // ============================================================

        if (request.IsLocked.HasValue)
        {
            query = query.Where(user =>
                user.IsLocked ==
                request.IsLocked.Value);
        }


        // ============================================================
        // Deleted Status
        // ============================================================

        if (request.IsDeleted.HasValue)
        {
            query = query.Where(user =>
                user.IsDeleted ==
                request.IsDeleted.Value);
        }


        // ============================================================
        // Created From
        // ============================================================

        if (request.CreatedFrom.HasValue)
        {
            query = query.Where(user =>
                user.CreatedAt >=
                request.CreatedFrom.Value);
        }


        // ============================================================
        // Created To
        // ============================================================

        if (request.CreatedTo.HasValue)
        {
            query = query.Where(user =>
                user.CreatedAt <=
                request.CreatedTo.Value);
        }


        // ============================================================
        // Total Count
        // ============================================================

        var totalCount =
            await query.CountAsync(
                cancellationToken);


        // ============================================================
        // Sorting
        // ============================================================

        query =
            request.SortBy?.Trim().ToLowerInvariant()
            switch
            {
                "employeeid" =>
                    request.SortDescending
                        ? query.OrderByDescending(
                            user => user.EmployeeId)
                        : query.OrderBy(
                            user => user.EmployeeId),

                "fullname" =>
                    request.SortDescending
                        ? query.OrderByDescending(
                            user => user.FullName)
                        : query.OrderBy(
                            user => user.FullName),

                "email" =>
                    request.SortDescending
                        ? query.OrderByDescending(
                            user => user.Email)
                        : query.OrderBy(
                            user => user.Email),

                "username" =>
                    request.SortDescending
                        ? query.OrderByDescending(
                            user => user.Username)
                        : query.OrderBy(
                            user => user.Username),

                "createdat" =>
                    request.SortDescending
                        ? query.OrderByDescending(
                            user => user.CreatedAt)
                        : query.OrderBy(
                            user => user.CreatedAt),

                "lastloginat" =>
                    request.SortDescending
                        ? query.OrderByDescending(
                            user => user.LastLoginAt)
                        : query.OrderBy(
                            user => user.LastLoginAt),

                _ =>
                    query.OrderBy(
                        user => user.FullName)
            };


        // ============================================================
        // Pagination
        // ============================================================

        var skip =
            (request.PageNumber - 1) *
            request.PageSize;

        var items =
            await query
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync(
                    cancellationToken);


        return new UserQueryResult
        {
            Items = items,
            TotalCount = totalCount
        };
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
            .Where(user =>
                user.IsActive &&
                !user.IsDeleted)
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
            .Where(user =>
                !user.IsActive &&
                !user.IsDeleted)
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
            .Where(user =>
                user.DepartmentId == departmentId &&
                !user.IsDeleted)
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
            .Where(user =>
                user.UserRoles.Any(
                    userRole =>
                        userRole.RoleId == roleId) &&
                !user.IsDeleted)
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }



    public async Task<User?> GetWithProfileDetailsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(user => user.Department)
            .Include(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
            .FirstOrDefaultAsync(
                user =>
                    user.Id == userId &&
                    !user.IsDeleted,
                cancellationToken);
    }
}