using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetAssignments.Requests;

public class CompleteAssetAssignmentRequestDto
{
    public DateTime ActualReturnDate { get; set; }

    public AssetCondition ConditionAtReturn { get; set; }

    public string? Remarks { get; set; }
}