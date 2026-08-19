namespace UAMS.Application.DTOs.Reservations.Responses;

public class ReservationListResponseDto
{
    public List<ReservationResponseDto> Items { get; set; }
        = new();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}