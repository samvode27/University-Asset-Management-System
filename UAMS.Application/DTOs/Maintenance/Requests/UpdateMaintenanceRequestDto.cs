using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Maintenance.Requests;

public class UpdateMaintenanceRequestDto
{
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

    [MaxLength(1000)]
    public string? Remarks { get; set; }
}