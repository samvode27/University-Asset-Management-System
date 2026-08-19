using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetReturns.Responses;

public class AssetReturnResponseDto
{
    public Guid Id { get; set; }

    public string ReturnNumber { get; set; } = null!;

    public Guid AssetId { get; set; }

    public Guid AssetAssignmentId { get; set; }

    public Guid ReturnedById { get; set; }

    public Guid ReceivedById { get; set; }

    public DateTime ReturnDate { get; set; }

    public string? ReturnLocation { get; set; }

    public AssetReturnCondition Condition { get; set; }

    public string? InspectionNotes { get; set; }

    public Guid? InspectedById { get; set; }

    public DateTime? InspectionDate { get; set; }

    public bool DamageFound { get; set; }

    public Guid? DamageReportId { get; set; }

    public string? Remarks { get; set; }

    public AssetReturnStatus Status { get; set; }
}