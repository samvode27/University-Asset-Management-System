using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.Barcodes;

public class Barcode : AuditableEntity
{
    private Barcode()
    {
    }

    public Barcode(
        Guid assetId,
        string code,
        string encodedData,
        BarcodeFormat format,
        string? imagePath,
        DateTime generatedAt,
        DateTime? expiresAt)
    {
        AssetId = assetId;
        Code = code;
        EncodedData = encodedData;
        Format = format;
        ImagePath = imagePath;
        GeneratedAt = generatedAt;
        ExpiresAt = expiresAt;

        IsActive = true;
    }

    public Guid AssetId { get; private set; }

    public string Code { get; private set; } = null!;

    public string EncodedData { get; private set; } = null!;

    public BarcodeFormat Format { get; private set; }

    public string? ImagePath { get; private set; }

    public DateTime GeneratedAt { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public bool IsActive { get; private set; }

    public Asset Asset { get; private set; } = null!;


    public void Update(
        string encodedData,
        BarcodeFormat format,
        string? imagePath,
        DateTime? expiresAt)
    {
        EncodedData = encodedData;
        Format = format;
        ImagePath = imagePath;
        ExpiresAt = expiresAt;
    }


    public void UpdateImagePath(string? imagePath)
    {
        ImagePath = imagePath;
    }


    public void ChangeFormat(BarcodeFormat format)
    {
        Format = format;
    }


    public void Activate()
    {
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    public bool IsExpired()
    {
        return ExpiresAt.HasValue &&
               ExpiresAt.Value <= DateTime.UtcNow;
    }


    public bool IsValid()
    {
        return IsActive && !IsExpired();
    }


    public void MarkDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}