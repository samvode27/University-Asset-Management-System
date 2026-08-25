using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Barcodes;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class BarcodeRepository
    : GenericRepository<Barcode>, IBarcodeRepository
{
    public BarcodeRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Barcode By Code
    // ================================================================

    public virtual async Task<Barcode?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                barcode => barcode.Code == code,
                cancellationToken);
    }


    // ================================================================
    // Get Barcode By ID With Details
    // ================================================================

    public virtual async Task<Barcode?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(barcode => barcode.Asset)
            .FirstOrDefaultAsync(
                barcode => barcode.Id == id,
                cancellationToken);
    }


    // ================================================================
    // Get Barcode By Asset
    // ================================================================

    public virtual async Task<Barcode?> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                barcode => barcode.AssetId == assetId,
                cancellationToken);
    }


    // ================================================================
    // Get Active Barcode By Asset
    // ================================================================

    public virtual async Task<Barcode?> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                barcode =>
                    barcode.AssetId == assetId &&
                    (
                        barcode.ExpiresAt == null ||
                        barcode.ExpiresAt > now
                    ),
                cancellationToken);
    }
}