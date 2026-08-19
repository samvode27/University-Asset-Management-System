using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetReturns.Requests;

public class UpdateAssetReturnRequestDto
{
    public DateTime ReturnDate { get; set; }

    public string? ReturnLocation { get; set; }

    public AssetReturnCondition Condition { get; set; }

    public string? InspectionNotes { get; set; }

    public string? Remarks { get; set; }
}