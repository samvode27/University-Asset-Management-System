using Microsoft.AspNetCore.Identity;
using UAMS.Application.Interfaces.Services;

namespace UAMS.Infrastructure.Services;

public sealed class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _passwordHasher;

    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        return _passwordHasher.HashPassword(
            new object(),
            password);
    }

    public bool VerifyPassword(
        string password,
        string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        var result = _passwordHasher.VerifyHashedPassword(
            new object(),
            passwordHash,
            password);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}