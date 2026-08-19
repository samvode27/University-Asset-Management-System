using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Maintenance.Requests;

public class CreateMaintenanceRequestDto
{
    [Required]
    public Guid AssetId { get; set; }

    public Guid? DamageReportId { get; set; }

    [Required]
    public Guid RequestedById { get; set; }

    [Required]
    public MaintenanceType MaintenanceType { get; set; }

    [Required]
    [MaxLength(2000)]
    public string ProblemDescription { get; set; } = null!;

    [MaxLength(2000)]
    public string? MaintenanceDescription { get; set; }

    [MaxLength(2000)]
    public string? PartsUsed { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? EstimatedCost { get; set; }

    public DateTime? RequestedDate { get; set; }

    [MaxLength(1000)]
    public string? Remarks { get; set; }
}