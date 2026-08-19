namespace UAMS.Application.DTOs.AssetAssignments.Requests;

public class UpdateAssetAssignmentRequestDto
{
    public DateTime? ExpectedReturnDate { get; set; }

    public string? AssignmentLocation { get; set; }

    public string? Remarks { get; set; }
}