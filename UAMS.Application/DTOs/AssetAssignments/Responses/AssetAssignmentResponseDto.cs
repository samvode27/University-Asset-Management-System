using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetAssignments.Responses;

public class AssetAssignmentResponseDto
{
    public Guid Id { get; set; }

    public string AssignmentNumber { get; set; } = null!;

    public Guid AssetId { get; set; }

    public Guid AssetRequestId { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid AssignedById { get; set; }

    public DateTime AssignedDate { get; set; }

    public DateTime? ExpectedReturnDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    public string? AssignmentLocation { get; set; }

    public AssetCondition ConditionAtAssignment { get; set; }

    public string? Remarks { get; set; }

    public AssetAssignmentStatus Status { get; set; }

    public bool IsActive { get; set; }
}