using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;

namespace UAMS.Domain.Entities.QRCodes;

public class QRCode : AuditableEntity
{
    private QRCode()
    {
    }

    public QRCode(
        Guid assetId,
        string code,
        string encodedData,
        string? imagePath,
        DateTime generatedAt,
        DateTime? expiresAt)
    {
        AssetId = assetId;
        Code = code;
        EncodedData = encodedData;
        ImagePath = imagePath;
        GeneratedAt = generatedAt;
        ExpiresAt = expiresAt;

        IsActive = true;
    }

    public Guid AssetId { get; private set; }

    public string Code { get; private set; } = null!;

    public string EncodedData { get; private set; } = null!;

    public string? ImagePath { get; private set; }

    public DateTime GeneratedAt { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public Asset Asset { get; private set; } = null!;


    public void Update(
        string encodedData,
        string? imagePath,
        DateTime? expiresAt)
    {
        EncodedData = encodedData;
        ImagePath = imagePath;
        ExpiresAt = expiresAt;
    }


    public void UpdateImagePath(string? imagePath)
    {
        ImagePath = imagePath;
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