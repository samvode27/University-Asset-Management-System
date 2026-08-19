using UAMS.Domain.Entities.DamageReports;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IDamageReportRepository
    : IRepository<DamageReport>
{
    // ================================================================
    // Get Damage Report By Report Number
    // ================================================================

    Task<DamageReport?> GetByReportNumberAsync(
        string reportNumber,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Damage Reports By Asset
    // ================================================================

    Task<IReadOnlyList<DamageReport>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Damage Reports By Asset Assignment
    // ================================================================

    Task<IReadOnlyList<DamageReport>> GetByAssetAssignmentIdAsync(
        Guid assetAssignmentId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Damage Reports By Reported By User
    // ================================================================

    Task<IReadOnlyList<DamageReport>> GetByReportedByIdAsync(
        Guid reportedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Damage Reports By Assessed By User
    // ================================================================

    Task<IReadOnlyList<DamageReport>> GetByAssessedByIdAsync(
        Guid assessedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Damage Reports By Damage Type
    // ================================================================

    Task<IReadOnlyList<DamageReport>> GetByDamageTypeAsync(
        DamageType damageType,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Damage Reports By Severity
    // ================================================================

    Task<IReadOnlyList<DamageReport>> GetBySeverityAsync(
        DamageSeverity severity,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Damage Reports By Status
    // ================================================================

    Task<IReadOnlyList<DamageReport>> GetByStatusAsync(
        DamageReportStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Open Damage Reports
    // ================================================================

    Task<IReadOnlyList<DamageReport>> GetOpenAsync(
        CancellationToken cancellationToken = default);
}