namespace UAMS.Application.DTOs.QRCode.Responses;

public class QRCodeResponseDto
{
    public Guid Id { get; set; }

    public Guid AssetId { get; set; }

    public string Code { get; set; } = null!;

    public string EncodedData { get; set; } = null!;

    public string? ImagePath { get; set; }

    public DateTime GeneratedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsExpired { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}