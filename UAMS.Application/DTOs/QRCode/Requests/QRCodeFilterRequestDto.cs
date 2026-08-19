namespace UAMS.Application.DTOs.QRCode.Requests;

public class QRCodeFilterRequestDto
{
    /// <summary>
    /// Filter by asset.
    /// </summary>
    public Guid? AssetId { get; set; }

    /// <summary>
    /// Search by QR code value.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Filter by active state.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Filter QR codes generated from this date.
    /// </summary>
    public DateTime? GeneratedFrom { get; set; }

    /// <summary>
    /// Filter QR codes generated until this date.
    /// </summary>
    public DateTime? GeneratedTo { get; set; }

    /// <summary>
    /// Filter QR codes that expire from this date.
    /// </summary>
    public DateTime? ExpiresFrom { get; set; }

    /// <summary>
    /// Filter QR codes that expire until this date.
    /// </summary>
    public DateTime? ExpiresTo { get; set; }

    /// <summary>
    /// Include only QR codes that have expired.
    /// </summary>
    public bool? IsExpired { get; set; }

    /// <summary>
    /// Page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of records per page.
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Sorting property.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction.
    /// </summary>
    public bool SortDescending { get; set; }
}