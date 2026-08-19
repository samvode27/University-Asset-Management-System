namespace UAMS.Application.DTOs.QRCode.Responses;

public class QRCodeDetailResponseDto
{
    public Guid Id { get; set; }

    // ============================================================
    // QR Code
    // ============================================================

    public string Code { get; set; } = null!;

    public string EncodedData { get; set; } = null!;

    public string? ImagePath { get; set; }

    public DateTime GeneratedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsExpired { get; set; }

    // ============================================================
    // Asset
    // ============================================================

    public Guid AssetId { get; set; }

    public string? AssetTag { get; set; }

    public string? AssetName { get; set; }

    public string? SerialNumber { get; set; }

    public string? AssetStatus { get; set; }

    // ============================================================
    // Audit
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}