using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Reservations.Responses;

public class ReservationConflictDto
{
    public Guid ReservationId { get; set; }

    public string ReservationNumber { get; set; } = null!;

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public ReservationStatus Status { get; set; }
}