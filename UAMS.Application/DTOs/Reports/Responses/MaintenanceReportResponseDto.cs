namespace UAMS.Application.DTOs.Reports.Responses;

public class MaintenanceReportResponseDto
{
    public Guid MaintenanceId { get; set; }

    public string MaintenanceNumber { get; set; } = null!;

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string MaintenanceType { get; set; } = null!;

    public string ProblemDescription { get; set; } = null!;

    public string? TechnicianName { get; set; }

    public decimal? EstimatedCost { get; set; }

    public decimal? ActualCost { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string? Result { get; set; }

    public string Status { get; set; } = null!;
}