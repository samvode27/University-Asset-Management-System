using UAMS.Domain.Common;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Enums;

namespace UAMS.Domain.Entities.Barcodes;

public class Barcode : AuditableEntity
{
    private Barcode()
    {
    }

    public Guid AssetId { get; private set; }

    public string Code { get; private set; } = null!;

    public string EncodedData { get; private set; } = null!;

    public BarcodeFormat Format { get; private set; }

    public string? ImagePath { get; private set; }

    public DateTime GeneratedAt { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public Asset Asset { get; private set; } = null!;

}