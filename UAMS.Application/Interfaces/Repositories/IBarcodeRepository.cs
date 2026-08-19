using UAMS.Domain.Entities.Barcodes;

namespace UAMS.Application.Interfaces.Repositories;

public interface IBarcodeRepository : IRepository<Barcode>
{
    // ================================================================
    // Barcode Lookup
    // ================================================================

    Task<Barcode?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Asset-Based Lookup
    // ================================================================

    Task<Barcode?> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Active Barcode
    // ================================================================

    Task<Barcode?> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);
}