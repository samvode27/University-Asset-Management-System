namespace UAMS.Application.DTOs.Reservations.Requests;

public class CreateReservationRequestDto
{
    public Guid AssetId { get; set; }

    public DateTime ReservationStartDate { get; set; }

    public DateTime ReservationEndDate { get; set; }

    public string? Purpose { get; set; }

    public string? Remarks { get; set; }
}