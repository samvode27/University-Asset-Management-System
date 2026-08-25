using UAMS.Application.DTOs.AuditLogs.Requests;
using UAMS.Application.DTOs.AuditLogs.Responses;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Services;

public interface IAuditLogService
{
    // ================================================================
    // Get Audit Log By ID
    // ================================================================

    Task<AuditLogDetailResponseDto?> GetByIdAsync(
        Guid auditLogId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs With Filter
    // ================================================================

    Task<AuditLogListResponseDto> GetAllAsync(
        AuditLogFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By User
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By Entity
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetByEntityAsync(
        string entityName,
        Guid? entityId = null,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By Action
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetByActionAsync(
        AuditAction action,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By Severity
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetBySeverityAsync(
        AuditSeverity severity,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs By Request ID
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetByRequestIdAsync(
        string requestId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Audit Logs Within Date Range
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetByDateRangeAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Failed Audit Logs
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetFailedAsync(
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Critical Audit Logs
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetCriticalAsync(
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Recent Audit Logs
    // ================================================================

    Task<IReadOnlyList<AuditLogResponseDto>> GetRecentAsync(
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