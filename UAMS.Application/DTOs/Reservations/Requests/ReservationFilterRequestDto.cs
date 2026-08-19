using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Reservations.Requests;

public class ReservationFilterRequestDto
{
    public Guid? AssetId { get; set; }

    public Guid? EmployeeId { get; set; }

    public Guid? DepartmentId { get; set; }

    public ReservationStatus? Status { get; set; }

    public DateTime? StartDateFrom { get; set; }

    public DateTime? StartDateTo { get; set; }

    public DateTime? EndDateFrom { get; set; }

    public DateTime? EndDateTo { get; set; }

    public string? SearchTerm { get; set; }

    public bool? IsActive { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}