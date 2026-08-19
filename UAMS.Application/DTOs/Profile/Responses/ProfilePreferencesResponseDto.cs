namespace UAMS.Application.DTOs.Profile.Responses;

public class ProfilePreferencesResponseDto
{
    public string? Language { get; set; }

    public string? TimeZone { get; set; }

    public bool EmailNotificationsEnabled { get; set; }

    public bool SystemNotificationsEnabled { get; set; }
}