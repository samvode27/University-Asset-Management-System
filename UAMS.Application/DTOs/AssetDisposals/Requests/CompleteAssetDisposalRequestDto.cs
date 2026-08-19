using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetDisposals.Requests;

public class CompleteAssetDisposalRequestDto
{
    // ============================================================
    // Disposal Method
    // ============================================================

    [Required]
    public DisposalMethod DisposalMethod { get; set; }


    // ============================================================
    // Actual Disposal Value
    // ============================================================

    [Range(0, double.MaxValue)]
    public decimal? DisposalValue { get; set; }


    // ============================================================
    // Disposal Remarks
    // ============================================================

    [MaxLength(2000)]
    public string? Remarks { get; set; }
}