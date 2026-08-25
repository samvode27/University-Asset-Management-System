using UAMS.Domain.Entities.Purchases;

namespace UAMS.Application.Interfaces.Repositories;

public interface IPurchaseRepository : IRepository<Purchase>
{
    // ================================================================
    // Purchase Lookup
    // ================================================================

    Task<Purchase?> GetByPurchaseNumberAsync(
        string purchaseNumber,
        CancellationToken cancellationToken = default);

    Task<Purchase?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Supplier-Based Queries
    // ================================================================

    Task<IReadOnlyList<Purchase>> GetBySupplierAsync(
        Guid supplierId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Purchase Date Queries
    // ================================================================

    Task<IReadOnlyList<Purchase>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);
}