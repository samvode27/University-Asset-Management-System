using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Maintenance.Requests;

public class MaintenanceFilterRequestDto
{
    public string? MaintenanceNumber { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? DamageReportId { get; set; }

    public Guid? RequestedById { get; set; }

    public Guid? AssignedTechnicianId { get; set; }

    public MaintenanceType? MaintenanceType { get; set; }

    public MaintenanceStatus? Status { get; set; }

    public MaintenanceResult? Result { get; set; }

    public DateTime? RequestedFromDate { get; set; }

    public DateTime? RequestedToDate { get; set; }

    public DateTime? CompletedFromDate { get; set; }

    public DateTime? CompletedToDate { get; set; }

    public bool? IsActive { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string? SearchTerm { get; set; }
}