using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Purchases;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class PurchaseRepository
    : GenericRepository<Purchase>, IPurchaseRepository
{
    public PurchaseRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Purchase By Purchase Number
    // ================================================================

    public virtual async Task<Purchase?> GetByPurchaseNumberAsync(
        string purchaseNumber,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(purchaseNumber);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                purchase => purchase.PurchaseNumber == purchaseNumber,
                cancellationToken);
    }


    // ================================================================
    // Get Purchases By Supplier
    // ================================================================

    public virtual async Task<IReadOnlyList<Purchase>>
        GetBySupplierAsync(
            Guid supplierId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(purchase => purchase.SupplierId == supplierId)
            .OrderByDescending(purchase => purchase.PurchaseDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Purchases By Date Range
    // ================================================================

    public virtual async Task<IReadOnlyList<Purchase>>
        GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException(
                "Start date cannot be greater than end date.",
                nameof(startDate));
        }

        var endDateExclusive = endDate.Date.AddDays(1);

        return await DbSet
            .AsNoTracking()
            .Where(purchase =>
                purchase.PurchaseDate >= startDate &&
                purchase.PurchaseDate < endDateExclusive)
            .OrderByDescending(purchase => purchase.PurchaseDate)
            .ToListAsync(cancellationToken);
    }
}