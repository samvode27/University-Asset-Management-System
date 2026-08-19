using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetAssignments.Requests;

public class AssetAssignmentFilterRequestDto
{
    public Guid? AssetId { get; set; }

    public Guid? AssetRequestId { get; set; }

    public Guid? EmployeeId { get; set; }

    public Guid? AssignedById { get; set; }

    public AssetAssignmentStatus? Status { get; set; }

    public DateTime? AssignedDateFrom { get; set; }

    public DateTime? AssignedDateTo { get; set; }

    public DateTime? ExpectedReturnDateFrom { get; set; }

    public DateTime? ExpectedReturnDateTo { get; set; }

    public string? SearchTerm { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}