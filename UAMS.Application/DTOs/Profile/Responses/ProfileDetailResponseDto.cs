namespace UAMS.Application.DTOs.Profile.Responses;

public class ProfileDetailResponseDto
{
    public ProfileResponseDto Profile { get; set; }
        = new();

    public ProfilePictureResponseDto? ProfilePicture { get; set; }

    public ProfilePreferencesResponseDto? Preferences { get; set; }

    public List<ProfileActivityDto> RecentActivities { get; set; }
        = new();
}