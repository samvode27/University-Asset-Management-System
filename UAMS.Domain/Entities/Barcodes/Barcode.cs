using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.Barcodes;

public class Barcode : AuditableEntity
{
    private Barcode()
    {
    }


    // ================================================================
    // Properties
    // ================================================================

    public Guid AssetId { get; private set; }

    public string Code { get; private set; } = null!;

    public string EncodedData { get; private set; } = null!;

    public BarcodeFormat Format { get; private set; }

    public string? ImagePath { get; private set; }

    public DateTime GeneratedAt { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public Asset Asset { get; private set; } = null!;


    // ================================================================
    // Factory
    // ================================================================

    public static Barcode Create(
        Guid assetId,
        string code,
        string encodedData,
        BarcodeFormat format,
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

        ArgumentException.ThrowIfNullOrWhiteSpace(
            code,
            nameof(code));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            encodedData,
            nameof(encodedData));

        if (!Enum.IsDefined(format))
        {
            throw new ArgumentException(
                "Invalid barcode format.",
                nameof(format));
        }

        if (expiresAt.HasValue &&
            expiresAt.Value <= generatedAt)
        {
            throw new ArgumentException(
                "Barcode expiration date must be after the generation date.",
                nameof(expiresAt));
        }

        return new Barcode
        {
            Id = Guid.NewGuid(),
            AssetId = assetId,
            Code = code.Trim(),
            EncodedData = encodedData.Trim(),
            Format = format,
            ImagePath = imagePath,
            GeneratedAt = generatedAt,
            ExpiresAt = expiresAt
        };
    }


    // ================================================================
    // Update
    // ================================================================

    public void Update(
        BarcodeFormat format,
        DateTime? expiresAt)
    {
        if (!Enum.IsDefined(format))
        {
            throw new ArgumentException(
                "Invalid barcode format.",
                nameof(format));
        }

        if (expiresAt.HasValue &&
            expiresAt.Value <= GeneratedAt)
        {
            throw new ArgumentException(
                "Barcode expiration date must be after the generation date.",
                nameof(expiresAt));
        }

        Format = format;
        ExpiresAt = expiresAt;
    }


    // ================================================================
    // Expiration
    // ================================================================

    public bool IsCurrentlyActive()
    {
        return ExpiresAt == null ||
               ExpiresAt > DateTime.UtcNow;
    }


    public bool IsExpired()
    {
        return ExpiresAt.HasValue &&
               ExpiresAt.Value <= DateTime.UtcNow;
    }
}