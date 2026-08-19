namespace UAMS.Application.DTOs.QRCode.Requests;

public class UpdateQRCodeRequestDto
{
    /// <summary>
    /// Optional expiration date.
    /// Null means the QR code does not expire.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}