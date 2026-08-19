using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.DamageReports.Requests;

public class UpdateDamageReportRequestDto
{
    [Required]
    public DamageType DamageType { get; set; }

    [Required]
    public DamageSeverity Severity { get; set; }

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = null!;

    public DateTime? IncidentDate { get; set; }

    [StringLength(500)]
    public string? IncidentLocation { get; set; }

    [StringLength(1000)]
    public string? Remarks { get; set; }
}