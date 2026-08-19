namespace UAMS.Application.DTOs.SystemSettings.Responses;

public class SystemSettingResponseDto
{
    public Guid Id { get; set; }

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Description { get; set; }

    public string Category { get; set; } = null!;

    public string DataType { get; set; } = null!;

    public bool IsEditable { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}