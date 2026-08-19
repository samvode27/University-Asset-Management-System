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
}