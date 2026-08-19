using System.ComponentModel.DataAnnotations;
using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetDisposals.Requests;

public class CreateAssetDisposalRequestDto
{
    // ============================================================
    // Asset
    // ============================================================

    [Required]
    public Guid AssetId { get; set; }


    // ============================================================
    // Maintenance
    // ============================================================

    public Guid? MaintenanceId { get; set; }


    // ============================================================
    // Disposal Information
    // ============================================================

    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = null!;


    [Range(0, double.MaxValue)]
    public decimal? BookValue { get; set; }


    [Range(0, double.MaxValue)]
    public decimal? EstimatedValue { get; set; }


    // ============================================================
    // Remarks
    // ============================================================

    [MaxLength(2000)]
    public string? Remarks { get; set; }
}