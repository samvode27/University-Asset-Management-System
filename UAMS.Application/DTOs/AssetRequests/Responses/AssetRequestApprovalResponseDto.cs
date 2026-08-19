using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetRequests.Responses;

public class AssetRequestApprovalResponseDto
{
    public Guid Id { get; set; }

    public string RequestNumber { get; set; } = null!;

    public AssetRequestStatus Status { get; set; }

    public bool Approved { get; set; }

    public Guid ActionedById { get; set; }

    public string? ActionedByName { get; set; }

    public DateTime ActionDate { get; set; }

    public string? Remarks { get; set; }

    public string? RejectionReason { get; set; }

    public bool RequiresNextApproval { get; set; }

    public bool ReadyForAssignment { get; set; }
}