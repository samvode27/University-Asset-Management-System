namespace UAMS.Application.DTOs.Permission.Requests;

public class PermissionFilterRequestDto
{
    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? Module { get; set; }

    public string? SearchTerm { get; set; }

    public bool? IsActive { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}

