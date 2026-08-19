using UAMS.Domain.Entities.Roles;

namespace UAMS.Application.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    // ================================================================
    // Role-specific queries
    // ================================================================

    Task<Role?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Role>> GetActiveRolesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Role>> GetSystemRolesAsync(
        CancellationToken cancellationToken = default);
}