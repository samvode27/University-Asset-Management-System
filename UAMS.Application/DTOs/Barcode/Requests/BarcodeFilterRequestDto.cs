using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Barcode.Requests;

public class BarcodeFilterRequestDto
{
    /// <summary>
    /// Filter by asset.
    /// </summary>
    public Guid? AssetId { get; set; }

    /// <summary>
    /// Search by barcode value.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Filter by barcode format.
    /// </summary>
    public BarcodeFormat? Format { get; set; }

    /// <summary>
    /// Filter by active state.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Filter barcodes generated from this date.
    /// </summary>
    public DateTime? GeneratedFrom { get; set; }

    /// <summary>
    /// Filter barcodes generated until this date.
    /// </summary>
    public DateTime? GeneratedTo { get; set; }

    /// <summary>
    /// Filter barcodes expiring from this date.
    /// </summary>
    public DateTime? ExpiresFrom { get; set; }

    /// <summary>
    /// Filter barcodes expiring until this date.
    /// </summary>
    public DateTime? ExpiresTo { get; set; }

    /// <summary>
    /// Filter expired barcodes.
    /// </summary>
    public bool? IsExpired { get; set; }

    /// <summary>
    /// Page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Property used for sorting.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Indicates whether sorting should be descending.
    /// </summary>
    public bool SortDescending { get; set; }
}