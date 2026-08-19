using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetReturns.Requests;

public class AssetReturnFilterRequestDto
{
    public string? ReturnNumber { get; set; }

    public Guid? AssetId { get; set; }

    public Guid? AssetAssignmentId { get; set; }

    public Guid? ReturnedById { get; set; }

    public Guid? ReceivedById { get; set; }

    public Guid? InspectedById { get; set; }

    public Guid? DamageReportId { get; set; }

    public AssetReturnCondition? Condition { get; set; }

    public bool? DamageFound { get; set; }

    public AssetReturnStatus? Status { get; set; }

    public DateTime? ReturnDateFrom { get; set; }

    public DateTime? ReturnDateTo { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string? SearchTerm { get; set; }
}