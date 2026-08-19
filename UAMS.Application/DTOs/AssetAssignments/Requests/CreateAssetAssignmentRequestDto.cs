using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetAssignments.Requests;

public class CreateAssetAssignmentRequestDto
{
    public Guid AssetId { get; set; }

    public Guid AssetRequestId { get; set; }

    public Guid EmployeeId { get; set; }

    public DateTime? ExpectedReturnDate { get; set; }

    public string? AssignmentLocation { get; set; }

    public AssetCondition ConditionAtAssignment { get; set; }

    public string? Remarks { get; set; }
}