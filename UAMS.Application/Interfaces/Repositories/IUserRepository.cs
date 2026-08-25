using UAMS.Application.DTOs.Users;
using UAMS.Application.DTOs.Users.Requests;
using UAMS.Domain.Entities.Users;

namespace UAMS.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    // ================================================================
    // User Lookup
    // ================================================================

    Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmployeeIdAsync(
        string employeeId,
        CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithAuthenticationDataAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<User?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    // ================================================================
    // User Filtering / Pagination
    // ================================================================

    Task<UserQueryResult> GetPagedAsync(
        UserFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Existence
    // ================================================================
    Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmployeeIdAsync(
        string employeeId,
        CancellationToken cancellationToken = default);

    // ================================================================
    // Authentication Lookup
    // ================================================================

    Task<User?> GetByUsernameForAuthenticationAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmailForAuthenticationAsync(
        string email,
        CancellationToken cancellationToken = default);


    // ================================================================
    // User Status
    // ================================================================

    Task<IReadOnlyList<User>> GetActiveUsersAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<User>> GetInactiveUsersAsync(
        CancellationToken cancellationToken = default);


    // ================================================================
    // Department
    // ================================================================

    Task<IReadOnlyList<User>> GetByDepartmentAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Role
    // ================================================================

    Task<IReadOnlyList<User>> GetByRoleAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    Task<User?> GetWithProfileDetailsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}