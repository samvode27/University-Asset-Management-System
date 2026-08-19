using UAMS.Domain.Entities.Permissions;

namespace UAMS.Application.Interfaces.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    // ================================================================
    // Permission-specific queries
    // ================================================================

    Task<Permission?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Permission>> GetByModuleAsync(
        string module,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Permission>> GetActivePermissionsAsync(
        CancellationToken cancellationToken = default);
}