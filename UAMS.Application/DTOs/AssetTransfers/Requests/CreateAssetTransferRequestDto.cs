using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.AssetTransfers.Requests;

public class CreateAssetTransferRequestDto
{
    [Required]
    public Guid AssetId { get; set; }

    [Required]
    public Guid AssetAssignmentId { get; set; }

    [Required]
    public Guid ToEmployeeId { get; set; }

    [Required]
    public Guid ToDepartmentId { get; set; }

    [MaxLength(500)]
    public string? ToLocation { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = null!;

    [MaxLength(1000)]
    public string? Remarks { get; set; }
}