using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.AssetDisposals.Requests;

public class ApproveAssetDisposalRequestDto
{
    // ============================================================
    // Disposal Method
    // ============================================================

    [Required]
    public UAMS.Domain.Enums.DisposalMethod DisposalMethod { get; set; }


    // ============================================================
    // Approval Information
    // ============================================================

    [MaxLength(2000)]
    public string? Remarks { get; set; }
}