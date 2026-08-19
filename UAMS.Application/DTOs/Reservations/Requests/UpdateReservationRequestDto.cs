namespace UAMS.Application.DTOs.Reservations.Requests;

public class UpdateReservationRequestDto
{
    public DateTime ReservationStartDate { get; set; }

    public DateTime ReservationEndDate { get; set; }

    public string? Purpose { get; set; }

    public string? Remarks { get; set; }
}