using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.AuditLogs;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class AuditLogRepository
    : GenericRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Audit Logs By User
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(log =>
                log.UserId == userId)
            .OrderByDescending(log =>
                log.Timestamp)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Audit Logs By Entity
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetByEntityAsync(
            string entityName,
            Guid? entityId = null,
            CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(log =>
                log.EntityName == entityName);

        if (entityId.HasValue)
        {
            query = query.Where(log =>
                log.EntityId == entityId.Value);
        }

        return await query
            .OrderByDescending(log =>
                log.Timestamp)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Audit Logs By Action
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetByActionAsync(
            AuditAction action,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(log =>
                log.Action == action)
            .OrderByDescending(log =>
                log.Timestamp)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Audit Logs By Severity
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetBySeverityAsync(
            AuditSeverity severity,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(log =>
                log.Severity == severity)
            .OrderByDescending(log =>
                log.Timestamp)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Audit Logs By Request ID
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetByRequestIdAsync(
            string requestId,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(requestId))
        {
            return Array.Empty<AuditLog>();
        }

        return await DbSet
            .AsNoTracking()
            .Where(log =>
                log.RequestId == requestId)
            .OrderBy(log =>
                log.Timestamp)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Audit Logs Within Date Range
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetByDateRangeAsync(
            DateTime from,
            DateTime to,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(log =>
                log.Timestamp >= from &&
                log.Timestamp <= to)
            .OrderByDescending(log =>
                log.Timestamp)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Failed Audit Logs
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetFailedAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(log =>
                !log.IsSuccessful)
            .OrderByDescending(log =>
                log.Timestamp)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Critical Audit Logs
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetCriticalAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(log =>
                log.Severity == AuditSeverity.Critical)
            .OrderByDescending(log =>
                log.Timestamp)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Recent Audit Logs
    // ================================================================

    public virtual async Task<IReadOnlyList<AuditLog>>
        GetRecentAsync(
            int count,
            CancellationToken cancellationToken = default)
    {
        if (count <= 0)
        {
            return Array.Empty<AuditLog>();
        }

        return await DbSet
            .AsNoTracking()
            .OrderByDescending(log =>
                log.Timestamp)
            .Take(count)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Count Audit Logs By User
    // ================================================================

    public virtual async Task<int>
        CountByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .CountAsync(
                log =>
                    log.UserId == userId,
                cancellationToken);
    }


    // ================================================================
    // Count Failed Audit Logs
    // ================================================================

    public virtual async Task<int>
        CountFailedAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .CountAsync(
                log =>
                    !log.IsSuccessful,
                cancellationToken);
    }
}