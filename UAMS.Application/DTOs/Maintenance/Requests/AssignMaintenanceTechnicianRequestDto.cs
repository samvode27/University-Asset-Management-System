using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Maintenance.Requests;

public class AssignMaintenanceTechnicianRequestDto
{
    [Required]
    public Guid AssignedTechnicianId { get; set; }

    [MaxLength(1000)]
    public string? Remarks { get; set; }
}