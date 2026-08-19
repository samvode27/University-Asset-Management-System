using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.DamageReports.Requests;

public class RejectDamageReportRequestDto
{
    [Required]
    [StringLength(2000)]
    public string RejectionReason { get; set; } = null!;
}