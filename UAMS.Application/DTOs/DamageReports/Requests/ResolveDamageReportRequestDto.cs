using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.DamageReports.Requests;

public class ResolveDamageReportRequestDto
{
    [StringLength(1000)]
    public string? ResolutionRemarks { get; set; }
}