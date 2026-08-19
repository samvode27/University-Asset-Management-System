using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.AssetTransfers.Requests;

public class RejectAssetTransferRequestDto
{
    [Required]
    [MaxLength(1000)]
    public string ApprovalRemarks { get; set; } = null!;
}