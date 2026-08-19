namespace UAMS.Application.DTOs.AssetReturns.Requests;

public class InspectAssetReturnRequestDto
{
    public Guid InspectedById { get; set; }

    public DateTime InspectionDate { get; set; }

    public bool DamageFound { get; set; }

    public string? InspectionNotes { get; set; }

    public Guid? DamageReportId { get; set; }

    public string? Remarks { get; set; }
}