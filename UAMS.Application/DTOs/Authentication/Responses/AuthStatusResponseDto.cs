namespace UAMS.Application.DTOs.Authentication.Responses;

public class AuthStatusResponseDto
{
    public bool IsAuthenticated { get; set; }

    public bool IsActive { get; set; }

    public bool IsLocked { get; set; }

    public CurrentUserResponseDto? User { get; set; }
}