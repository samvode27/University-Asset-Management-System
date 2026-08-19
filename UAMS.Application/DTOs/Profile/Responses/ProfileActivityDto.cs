namespace UAMS.Application.DTOs.Profile.Responses;

public class ProfileActivityDto
{
    public Guid Id { get; set; }

    public string Action { get; set; } = null!;

    public string EntityName { get; set; } = null!;

    public string? Description { get; set; }

    public string? IpAddress { get; set; }

    public DateTime Timestamp { get; set; }

    public bool IsSuccessful { get; set; }
}