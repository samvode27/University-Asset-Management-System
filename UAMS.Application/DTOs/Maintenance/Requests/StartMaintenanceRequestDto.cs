using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Maintenance.Requests;

public class StartMaintenanceRequestDto
{
    [MaxLength(2000)]
    public string? MaintenanceDescription { get; set; }

    [MaxLength(2000)]
    public string? PartsUsed { get; set; }

    [MaxLength(1000)]
    public string? Remarks { get; set; }
}