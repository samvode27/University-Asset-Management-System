namespace UAMS.Application.DTOs.Profile.Responses;

public class ProfilePictureResponseDto
{
    public Guid FileId { get; set; }

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long FileSize { get; set; }

    public string FileUrl { get; set; } = null!;

    public DateTime UploadedAt { get; set; }
}