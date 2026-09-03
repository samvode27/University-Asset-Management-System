using UAMS.Domain.Entities.Users;

namespace UAMS.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    // ============================================================
    // Add
    // ============================================================

    Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By Token Hash
    // ============================================================

    Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get Active Tokens By User
    // ============================================================

    Task<IReadOnlyList<RefreshToken>>
        GetActiveByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // Update
    // ============================================================

    void Update(
        RefreshToken refreshToken);
}