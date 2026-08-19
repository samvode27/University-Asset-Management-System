namespace UAMS.Application.DTOs.Authentication.Responses;

public class UserSessionResponseDto
{
    public Guid SessionId { get; set; }

    public Guid UserId { get; set; }

    public DateTime LoginAt { get; set; }

    public DateTime? LastActivityAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public bool IsActive { get; set; }
}