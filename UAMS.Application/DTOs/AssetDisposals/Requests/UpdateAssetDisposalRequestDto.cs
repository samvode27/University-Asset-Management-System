    using System.ComponentModel.DataAnnotations;

    namespace UAMS.Application.DTOs.AssetDisposals.Requests;

    public class UpdateAssetDisposalRequestDto
    {
        // ============================================================
        // Maintenance
        // ============================================================

        public Guid? MaintenanceId { get; set; }


        // ============================================================
        // Disposal Reason
        // ============================================================

        [Required]
        [MaxLength(1000)]
        public string Reason { get; set; } = null!;


        // ============================================================
        // Financial Information
        // ============================================================

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