using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetReturns.Requests;

public class CreateAssetReturnRequestDto
{
    public Guid AssetId { get; set; }

    public Guid AssetAssignmentId { get; set; }

    public Guid ReturnedById { get; set; }

    public Guid ReceivedById { get; set; }

    public DateTime ReturnDate { get; set; }

    public string? ReturnLocation { get; set; }

    public AssetReturnCondition Condition { get; set; }

    public string? InspectionNotes { get; set; }

    public string? Remarks { get; set; }
}