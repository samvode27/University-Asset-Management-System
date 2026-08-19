namespace UAMS.Application.DTOs.Profile.Responses;

public class ProfileSummaryResponseDto
{
    public Guid Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? DepartmentName { get; set; }

    public string? PrimaryRole { get; set; }

    public string? ProfilePictureUrl { get; set; }
}