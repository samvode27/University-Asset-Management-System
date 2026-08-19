namespace UAMS.Application.DTOs.SystemSettings.Responses;

public class SystemSettingGroupResponseDto
{
    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public List<SystemSettingResponseDto> Settings { get; set; }
        = new();
}