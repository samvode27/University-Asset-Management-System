using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Barcode.Requests;

public class UpdateBarcodeRequestDto
{
    /// <summary>
    /// Barcode format.
    /// </summary>
    public BarcodeFormat Format { get; set; }

    /// <summary>
    /// Optional expiration date.
    /// Null means the barcode does not expire.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}