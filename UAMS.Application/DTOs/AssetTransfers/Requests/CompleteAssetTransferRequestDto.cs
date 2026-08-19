using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.AssetTransfers.Requests;

public class CompleteAssetTransferRequestDto
{
    [MaxLength(1000)]
    public string? Remarks { get; set; }
}