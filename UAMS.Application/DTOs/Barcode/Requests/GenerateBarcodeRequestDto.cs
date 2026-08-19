using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Barcode.Requests;

public class GenerateBarcodeRequestDto
{
    /// <summary>
    /// Asset for which the barcode will be generated.
    /// </summary>
    public Guid AssetId { get; set; }

    /// <summary>
    /// Barcode format to generate.
    /// </summary>
    public BarcodeFormat Format { get; set; }

    /// <summary>
    /// Optional expiration date.
    /// Null means the barcode does not expire.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}