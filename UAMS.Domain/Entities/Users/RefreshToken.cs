namespace UAMS.Domain.Entities.Users;

public class RefreshToken
{
    public Guid Id { get; private set; }

    public string TokenHash { get; private set; } = null!;

    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public DateTime ExpiresAt { get; private set; }

    public bool IsRevoked { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public string? ReplacedByTokenHash { get; private set; }

    public DateTime CreatedAt { get; private set; }


    private RefreshToken()
    {
    }


    public static RefreshToken Create(
        string tokenHash,
        Guid userId,
        DateTime expiresAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            tokenHash);

        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(userId));
        }

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            TokenHash = tokenHash,
            UserId = userId,
            ExpiresAt = expiresAt,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };
    }


    public bool IsExpired =>
        DateTime.UtcNow >= ExpiresAt;


    public bool IsActive =>
        !IsRevoked && !IsExpired;


    public void Revoke(
        string? replacedByTokenHash = null)
    {
        if (IsRevoked)
        {
            return;
        }

        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}