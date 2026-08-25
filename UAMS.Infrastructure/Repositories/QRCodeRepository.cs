using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.QRCodes;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class QRCodeRepository
    : GenericRepository<QRCode>, IQRCodeRepository
{
    public QRCodeRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get QR Code By Code
    // ================================================================

    public virtual async Task<QRCode?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                qrCode => qrCode.Code == code,
                cancellationToken);
    }


    // ================================================================
    // Get QR Code By ID With Details
    // ================================================================

    public virtual async Task<QRCode?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "QR code ID is required.",
                nameof(id));
        }

        return await DbSet
            .AsNoTracking()
            .Include(qrCode => qrCode.Asset)
            .FirstOrDefaultAsync(
                qrCode => qrCode.Id == id,
                cancellationToken);
    }


    // ================================================================
    // Get QR Code By Asset
    // ================================================================

    public virtual async Task<QRCode?> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                qrCode => qrCode.AssetId == assetId,
                cancellationToken);
    }


    // ================================================================
    // Get Active QR Code By Asset
    // ================================================================

    public virtual async Task<QRCode?> GetActiveByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                qrCode =>
                    qrCode.AssetId == assetId &&
                    (
                        qrCode.ExpiresAt == null ||
                        qrCode.ExpiresAt > now
                    ),
                cancellationToken);
    }
}