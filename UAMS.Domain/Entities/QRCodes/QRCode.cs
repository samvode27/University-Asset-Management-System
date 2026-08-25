using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;

namespace UAMS.Domain.Entities.QRCodes;

public class QRCode : AuditableEntity
{
    private QRCode()
    {
    }

    public Guid AssetId { get; private set; }

    public string Code { get; private set; } = null!;

    public string EncodedData { get; private set; } = null!;

    public string? ImagePath { get; private set; }

    public DateTime GeneratedAt { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public Asset Asset { get; private set; } = null!;


    // ================================================================
    // Factory
    // ================================================================

    public static QRCode Create(
        Guid assetId,
        string code,
        string encodedData,
        string? imagePath,
        DateTime generatedAt,
        DateTime? expiresAt)
    {
        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(encodedData);

        if (expiresAt.HasValue &&
            expiresAt.Value <= generatedAt)
        {
            throw new ArgumentException(
                "QR code expiration date must be after the generation date.",
                nameof(expiresAt));
        }

        return new QRCode
        {
            AssetId = assetId,
            Code = code.Trim(),
            EncodedData = encodedData.Trim(),
            ImagePath = imagePath,
            GeneratedAt = generatedAt,
            ExpiresAt = expiresAt
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void UpdateExpiration(DateTime? expiresAt)
    {
        if (expiresAt.HasValue &&
            expiresAt.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "QR code expiration date must be in the future.",
                nameof(expiresAt));
        }

        ExpiresAt = expiresAt;
    }


    // ================================================================
    // Status
    // ================================================================

    public bool IsExpired()
    {
        return ExpiresAt.HasValue &&
               ExpiresAt.Value <= DateTime.UtcNow;
    }
}