namespace UAMS.Application.Options;

public sealed class AuthenticationOptions
{
    public const string SectionName = "Authentication";

    public int MaxFailedLoginAttempts { get; set; } = 5;
}