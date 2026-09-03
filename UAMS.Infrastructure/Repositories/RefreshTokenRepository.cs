using Microsoft.EntityFrameworkCore;
    
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Users;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class RefreshTokenRepository
    : IRefreshTokenRepository
{
    private readonly UAMSDbContext _context;

    public RefreshTokenRepository(
        UAMSDbContext context)
    {
        _context = context
            ?? throw new ArgumentNullException(
                nameof(context));
    }


    // ============================================================
    // Add
    // ============================================================

    public async Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);

        await _context.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);
    }


    // ============================================================
    // Get By Token Hash
    // ============================================================

    public async Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            tokenHash);

        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(
                rt => rt.TokenHash == tokenHash,
                cancellationToken);
    }


    // ============================================================
    // Get Active Tokens By User
    // ============================================================

    public async Task<IReadOnlyList<RefreshToken>>
        GetActiveByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(userId));
        }

        return await _context.RefreshTokens
            .Where(rt =>
                rt.UserId == userId &&
                !rt.IsRevoked &&
                rt.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(rt => rt.CreatedAt)
            .ToListAsync(cancellationToken);
    }


    // ============================================================
    // Update
    // ============================================================

    public void Update(
        RefreshToken refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);

        _context.RefreshTokens.Update(refreshToken);
    }
}