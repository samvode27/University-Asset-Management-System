using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Maintenance.Responses;

public class MaintenanceResponseDto
{
    public Guid Id { get; set; }

    public string MaintenanceNumber { get; set; } = null!;

    public Guid AssetId { get; set; }

    public Guid? DamageReportId { get; set; }

    public Guid RequestedById { get; set; }

    public Guid? AssignedTechnicianId { get; set; }

    public MaintenanceType MaintenanceType { get; set; }

    public string ProblemDescription { get; set; } = null!;

    public string? MaintenanceDescription { get; set; }

    public string? PartsUsed { get; set; }

    public decimal? EstimatedCost { get; set; }

    public decimal? ActualCost { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public MaintenanceResult? Result { get; set; }

    public string? FailureReason { get; set; }

    public string? Remarks { get; set; }

    public MaintenanceStatus Status { get; set; }

    public bool IsActive { get; set; }
}