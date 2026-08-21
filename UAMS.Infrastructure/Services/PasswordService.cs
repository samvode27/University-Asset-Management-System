using Microsoft.AspNetCore.Identity;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Users;

namespace UAMS.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _passwordHasher;

    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<User>();
    }


    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        return _passwordHasher.HashPassword(
            null!,
            password);
    }


    public bool VerifyPassword(
        string password,
        string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        var result = _passwordHasher.VerifyHashedPassword(
            null!,
            passwordHash,
            password);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}