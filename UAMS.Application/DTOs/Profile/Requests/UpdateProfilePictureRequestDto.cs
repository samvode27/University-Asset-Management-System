namespace UAMS.Application.DTOs.Profile.Requests;

public class UpdateProfilePictureRequestDto
{
    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long FileSize { get; set; }

    public string FilePath { get; set; } = null!;
}