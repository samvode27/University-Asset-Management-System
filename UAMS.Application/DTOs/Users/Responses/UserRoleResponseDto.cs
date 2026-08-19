namespace UAMS.Application.DTOs.Users.Responses;

public class UserRoleResponseDto
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = null!;
}