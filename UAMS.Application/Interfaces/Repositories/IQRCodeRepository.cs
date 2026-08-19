using UAMS.Domain.Entities.QRCodes;

namespace UAMS.Application.Interfaces.Repositories;

public interface IQRCodeRepository : IRepository<QRCode>
{
    // ================================================================
    // QR Code Lookup
    // ================================================================

    Task<QRCode?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Asset-Based Lookup
    // ================================================================

    Task<QRCode?> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Active QR Code
    // ================================================================

    Task<QRCode?> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);
}