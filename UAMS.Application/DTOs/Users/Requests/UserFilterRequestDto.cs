using System.ComponentModel.DataAnnotations;

namespace UAMS.Application.DTOs.Users.Requests;

public class UserFilterRequestDto
{
    public string? Search { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? RoleId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsLocked { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedFrom { get; set; }

    public DateTime? CreatedTo { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; }

    public bool SortDescending { get; set; }
}