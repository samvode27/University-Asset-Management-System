namespace UAMS.Application.DTOs.Dashboard.Responses;

public class RecentActivityDto
{
    public Guid Id { get; set; }

    public string Action { get; set; } = null!;

    public string EntityName { get; set; } = null!;

    public Guid? EntityId { get; set; }

    public string Description { get; set; } = null!;

    public string? UserName { get; set; }

    public DateTime Timestamp { get; set; }

    public bool IsSuccessful { get; set; }
}