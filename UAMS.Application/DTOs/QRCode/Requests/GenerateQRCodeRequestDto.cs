namespace UAMS.Application.DTOs.QRCode.Requests;

public class GenerateQRCodeRequestDto
{
    /// <summary>
    /// Asset for which the QR code will be generated.
    /// </summary>
    public Guid AssetId { get; set; }

    /// <summary>
    /// Optional expiration date for the QR code.
    /// Null means the QR code does not expire.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}