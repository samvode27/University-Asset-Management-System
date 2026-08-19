using UAMS.Domain.Entities.AuditLogs;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IAuditLogRepository
    : IRepository<AuditLog>
{
    // ================================================================
    // Get Audit Logs By User
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By Entity
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetByEntityAsync(
        string entityName,
        Guid? entityId = null,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By Action
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetByActionAsync(
        AuditAction action,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By Severity
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetBySeverityAsync(
        AuditSeverity severity,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By Request ID
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetByRequestIdAsync(
        string requestId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs Within Date Range
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetByDateRangeAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Failed Audit Logs
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetFailedAsync(
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Critical Audit Logs
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetCriticalAsync(
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Recent Audit Logs
    // ================================================================

    Task<IReadOnlyList<AuditLog>> GetRecentAsync(
        int count,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Count Audit Logs By User
    // ================================================================

    Task<int> CountByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Count Failed Audit Logs
    // ================================================================

    Task<int> CountFailedAsync(
        CancellationToken cancellationToken = default);
}