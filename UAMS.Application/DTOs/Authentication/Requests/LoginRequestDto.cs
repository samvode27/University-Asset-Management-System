namespace UAMS.Application.DTOs.Authentication.Requests;

public class LoginRequestDto
{
    public string UsernameOrEmail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}