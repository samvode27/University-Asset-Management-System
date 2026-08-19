using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.DamageReports;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class DamageReportRepository
    : GenericRepository<DamageReport>, IDamageReportRepository
{
    public DamageReportRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Damage Report By Report Number
    // ================================================================

    public virtual async Task<DamageReport?>
        GetByReportNumberAsync(
            string reportNumber,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                report =>
                    report.ReportNumber == reportNumber,
                cancellationToken);
    }


    // ================================================================
    // Get Damage Reports By Asset
    // ================================================================

    public virtual async Task<IReadOnlyList<DamageReport>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(report =>
                report.AssetId == assetId)
            .OrderByDescending(report =>
                report.ReportedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Damage Reports By Asset Assignment
    // ================================================================

    public virtual async Task<IReadOnlyList<DamageReport>>
        GetByAssetAssignmentIdAsync(
            Guid assetAssignmentId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(report =>
                report.AssetAssignmentId == assetAssignmentId)
            .OrderByDescending(report =>
                report.ReportedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Damage Reports By Reported By User
    // ================================================================

    public virtual async Task<IReadOnlyList<DamageReport>>
        GetByReportedByIdAsync(
            Guid reportedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(report =>
                report.ReportedById == reportedById)
            .OrderByDescending(report =>
                report.ReportedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Damage Reports By Assessed By User
    // ================================================================

    public virtual async Task<IReadOnlyList<DamageReport>>
        GetByAssessedByIdAsync(
            Guid assessedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(report =>
                report.AssessedById == assessedById)
            .OrderByDescending(report =>
                report.AssessedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Damage Reports By Damage Type
    // ================================================================

    public virtual async Task<IReadOnlyList<DamageReport>>
        GetByDamageTypeAsync(
            DamageType damageType,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(report =>
                report.DamageType == damageType)
            .OrderByDescending(report =>
                report.ReportedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Damage Reports By Severity
    // ================================================================

    public virtual async Task<IReadOnlyList<DamageReport>>
        GetBySeverityAsync(
            DamageSeverity severity,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(report =>
                report.Severity == severity)
            .OrderByDescending(report =>
                report.ReportedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Damage Reports By Status
    // ================================================================

    public virtual async Task<IReadOnlyList<DamageReport>>
        GetByStatusAsync(
            DamageReportStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(report =>
                report.Status == status)
            .OrderByDescending(report =>
                report.ReportedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Open Damage Reports
    // ================================================================

    public virtual async Task<IReadOnlyList<DamageReport>>
        GetOpenAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(report =>
                report.Status == DamageReportStatus.Submitted ||
                report.Status == DamageReportStatus.UnderReview ||
                report.Status == DamageReportStatus.MaintenanceRequired)
            .OrderBy(report =>
                report.ReportedDate)
            .ToListAsync(cancellationToken);
    }
}