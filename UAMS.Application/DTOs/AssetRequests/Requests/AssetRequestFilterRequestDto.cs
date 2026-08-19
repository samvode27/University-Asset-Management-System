using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetRequests.Requests;

public class AssetRequestFilterRequestDto
{
    /// <summary>
    /// Request number search.
    /// </summary>
    public string? RequestNumber { get; set; }

    /// <summary>
    /// Filter by asset.
    /// </summary>
    public Guid? AssetId { get; set; }

    /// <summary>
    /// Filter by requester.
    /// </summary>
    public Guid? RequesterId { get; set; }

    /// <summary>
    /// Filter by department.
    /// </summary>
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// Filter by request status.
    /// </summary>
    public AssetRequestStatus? Status { get; set; }

    /// <summary>
    /// Requests created from this date.
    /// </summary>
    public DateTime? RequestedFrom { get; set; }

    /// <summary>
    /// Requests created until this date.
    /// </summary>
    public DateTime? RequestedTo { get; set; }

    /// <summary>
    /// Filter requests requiring Department Head action.
    /// </summary>
    public bool? RequiresDepartmentHeadAction { get; set; }

    /// <summary>
    /// Filter requests requiring Asset Manager action.
    /// </summary>
    public bool? RequiresAssetManagerAction { get; set; }

    /// <summary>
    /// Page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Sorting property.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort descending.
    /// </summary>
    public bool SortDescending { get; set; }
}