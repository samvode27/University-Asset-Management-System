using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetRequests.Responses;

public class AssetRequestResponseDto
{
    public Guid Id { get; set; }

    public string RequestNumber { get; set; } = null!;

    public Guid AssetId { get; set; }

    public Guid RequesterId { get; set; }

    public Guid DepartmentId { get; set; }

    public string Purpose { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public DateTime? RequiredFromDate { get; set; }

    public DateTime? RequiredToDate { get; set; }

    public AssetRequestStatus Status { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}