namespace UAMS.Application.DTOs.SystemSettings.Requests;

public class CreateSystemSettingRequestDto
{
    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Description { get; set; }

    public string Category { get; set; } = null!;

    public string DataType { get; set; } = null!;

    public bool IsEditable { get; set; } = true;
}