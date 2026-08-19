using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Maintenance.Requests;

public class CancelMaintenanceRequestDto
{
    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = null!;
}