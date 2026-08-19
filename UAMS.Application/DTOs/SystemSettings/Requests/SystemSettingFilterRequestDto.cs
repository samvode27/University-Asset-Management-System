namespace UAMS.Application.DTOs.SystemSettings.Requests;

public class SystemSettingFilterRequestDto
{
    public string? SearchTerm { get; set; }

    public string? Category { get; set; }

    public string? DataType { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsEditable { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}