using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.DamageReports.Requests;

public class AssessDamageReportRequestDto
{
    [Required]
    [StringLength(2000)]
    public string Assessment { get; set; } = null!;

    [Required]
    public bool IsRepairable { get; set; }
}