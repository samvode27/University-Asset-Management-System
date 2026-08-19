namespace UAMS.Application.DTOs.Reservations.Responses;

public class AssetAvailabilityResponseDto
{
    public Guid AssetId { get; set; }

    public string AssetNumber { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public DateTime RequestedStartDate { get; set; }

    public DateTime RequestedEndDate { get; set; }

    public List<ReservationConflictDto> Conflicts { get; set; }
        = new();
}