using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.AssetDisposals.Requests;

public class RejectAssetDisposalRequestDto
{
    // ============================================================
    // Rejection Reason
    // ============================================================

    [Required]
    [MaxLength(2000)]
    public string Reason { get; set; } = null!;
}