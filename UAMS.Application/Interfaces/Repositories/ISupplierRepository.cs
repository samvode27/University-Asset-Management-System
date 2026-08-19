using UAMS.Domain.Entities.Suppliers;

namespace UAMS.Application.Interfaces.Repositories;

public interface ISupplierRepository : IRepository<Supplier>
{
    // ================================================================
    // Supplier Lookup
    // ================================================================

    Task<Supplier?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Supplier Status
    // ================================================================

    Task<IReadOnlyList<Supplier>> GetActiveSuppliersAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Supplier>> GetInactiveSuppliersAsync(
        CancellationToken cancellationToken = default);
}