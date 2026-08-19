using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Maintenance.Responses;

public class MaintenanceListResponseDto
{
    public Guid Id { get; set; }

    public string MaintenanceNumber { get; set; } = null!;

    public Guid AssetId { get; set; }

    public string? AssetTag { get; set; }

    public string? AssetName { get; set; }

    public MaintenanceType MaintenanceType { get; set; }

    public Guid RequestedById { get; set; }

    public string? RequestedByName { get; set; }

    public Guid? AssignedTechnicianId { get; set; }

    public string? AssignedTechnicianName { get; set; }

    public decimal? EstimatedCost { get; set; }

    public decimal? ActualCost { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public MaintenanceResult? Result { get; set; }

    public MaintenanceStatus Status { get; set; }

    public bool IsActive { get; set; }
}