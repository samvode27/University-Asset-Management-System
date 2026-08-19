using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Maintenance.Requests;

public class CompleteMaintenanceRequestDto
{
    [Required]
    public MaintenanceResult Result { get; set; }

    [Range(0, double.MaxValue)]
    public decimal ActualCost { get; set; }

    [MaxLength(2000)]
    public string? MaintenanceDescription { get; set; }

    [MaxLength(2000)]
    public string? PartsUsed { get; set; }

    [MaxLength(2000)]
    public string? FailureReason { get; set; }

    [MaxLength(1000)]
    public string? Remarks { get; set; }
}