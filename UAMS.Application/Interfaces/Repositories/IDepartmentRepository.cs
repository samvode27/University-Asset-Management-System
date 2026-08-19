using UAMS.Domain.Entities.Departments;

namespace UAMS.Application.Interfaces.Repositories;

public interface IDepartmentRepository : IRepository<Department>
{
    // ================================================================
    // Department Lookup
    // ================================================================

    Task<Department?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Department Status
    // ================================================================

    Task<IReadOnlyList<Department>> GetActiveDepartmentsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Department>> GetInactiveDepartmentsAsync(
        CancellationToken cancellationToken = default);
}