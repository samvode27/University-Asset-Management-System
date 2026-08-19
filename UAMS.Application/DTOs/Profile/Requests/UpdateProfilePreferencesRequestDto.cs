namespace UAMS.Application.DTOs.Profile.Requests;

public class UpdateProfilePreferencesRequestDto
{
    public string? Language { get; set; }

    public string? TimeZone { get; set; }

    public bool EmailNotificationsEnabled { get; set; }

    public bool SystemNotificationsEnabled { get; set; }
}