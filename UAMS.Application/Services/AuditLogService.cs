using UAMS.Application.DTOs.AuditLogs.Requests;
using UAMS.Application.DTOs.AuditLogs.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.AuditLogs;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services.AuditLogs;

public class AuditLogService : IAuditLogService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuditLogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // ================================================================
    // Get Audit Log By ID
    // ================================================================

    public async Task<AuditLogDetailResponseDto?> GetByIdAsync(
        Guid auditLogId,
        CancellationToken cancellationToken = default)
    {
        var auditLog = await _unitOfWork.AuditLogs
            .GetByIdAsync(
                auditLogId,
                cancellationToken);

        if (auditLog is null)
        {
            return null;
        }

        return MapToDetailResponse(auditLog);
    }


    // ================================================================
    // Get Audit Logs With Filter
    // ================================================================

    public async Task<AuditLogListResponseDto> GetAllAsync(
        AuditLogFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<AuditLog> auditLogs;

        // ------------------------------------------------------------
        // Choose the most specific repository query available.
        // ------------------------------------------------------------

        if (request.UserId.HasValue)
        {
            auditLogs = await _unitOfWork.AuditLogs
                .GetByUserIdAsync(
                    request.UserId.Value,
                    cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(request.EntityName))
        {
            auditLogs = await _unitOfWork.AuditLogs
                .GetByEntityAsync(
                    request.EntityName,
                    request.EntityId,
                    cancellationToken);
        }
        else if (request.Action.HasValue)
        {
            auditLogs = await _unitOfWork.AuditLogs
                .GetByActionAsync(
                    request.Action.Value,
                    cancellationToken);
        }
        else if (request.Severity.HasValue)
        {
            auditLogs = await _unitOfWork.AuditLogs
                .GetBySeverityAsync(
                    request.Severity.Value,
                    cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(request.RequestId))
        {
            auditLogs = await _unitOfWork.AuditLogs
                .GetByRequestIdAsync(
                    request.RequestId,
                    cancellationToken);
        }
        else if (request.FromDate.HasValue ||
                 request.ToDate.HasValue)
        {
            var from = request.FromDate
                       ?? DateTime.MinValue;

            var to = request.ToDate
                     ?? DateTime.MaxValue;

            auditLogs = await _unitOfWork.AuditLogs
                .GetByDateRangeAsync(
                    from,
                    to,
                    cancellationToken);
        }
        else
        {
            // No specific repository filter was supplied.
            // Use the full available date range.
            auditLogs = await _unitOfWork.AuditLogs
                .GetByDateRangeAsync(
                    DateTime.MinValue,
                    DateTime.MaxValue,
                    cancellationToken);
        }


        // ------------------------------------------------------------
        // Additional filters
        // ------------------------------------------------------------

        IEnumerable<AuditLog> filteredLogs = auditLogs;


        if (request.UserId.HasValue)
        {
            filteredLogs = filteredLogs.Where(x =>
                x.UserId == request.UserId.Value);
        }


        if (request.Action.HasValue)
        {
            filteredLogs = filteredLogs.Where(x =>
                x.Action == request.Action.Value);
        }


        if (!string.IsNullOrWhiteSpace(request.EntityName))
        {
            filteredLogs = filteredLogs.Where(x =>
                x.EntityName.Contains(
                    request.EntityName,
                    StringComparison.OrdinalIgnoreCase));
        }


        if (request.EntityId.HasValue)
        {
            filteredLogs = filteredLogs.Where(x =>
                x.EntityId == request.EntityId.Value);
        }


        if (request.Severity.HasValue)
        {
            filteredLogs = filteredLogs.Where(x =>
                x.Severity == request.Severity.Value);
        }


        if (request.IsSuccessful.HasValue)
        {
            filteredLogs = filteredLogs.Where(x =>
                x.IsSuccessful == request.IsSuccessful.Value);
        }


        if (!string.IsNullOrWhiteSpace(request.RequestId))
        {
            filteredLogs = filteredLogs.Where(x =>
                x.RequestId == request.RequestId);
        }


        if (request.FromDate.HasValue)
        {
            filteredLogs = filteredLogs.Where(x =>
                x.Timestamp >= request.FromDate.Value);
        }


        if (request.ToDate.HasValue)
        {
            filteredLogs = filteredLogs.Where(x =>
                x.Timestamp <= request.ToDate.Value);
        }


        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            filteredLogs = filteredLogs.Where(x =>
                x.Description.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase)
                ||
                x.EntityName.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase)
                ||
                (x.ErrorMessage != null &&
                 x.ErrorMessage.Contains(
                     searchTerm,
                     StringComparison.OrdinalIgnoreCase))
                ||
                (x.RequestId != null &&
                 x.RequestId.Contains(
                     searchTerm,
                     StringComparison.OrdinalIgnoreCase)));
        }


        // ------------------------------------------------------------
        // Ordering
        // ------------------------------------------------------------

        filteredLogs = filteredLogs
            .OrderByDescending(x => x.Timestamp);


        // ------------------------------------------------------------
        // Pagination
        // ------------------------------------------------------------

        var totalCount = filteredLogs.Count();

        var pageNumber = request.PageNumber;
        var pageSize = request.PageSize;

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(
                totalCount / (double)pageSize);

        var items = filteredLogs
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToResponse)
            .ToList();


        return new AuditLogListResponseDto
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }


    // ================================================================
    // Get Audit Logs By User
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetByUserIdAsync(
                userId,
                cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Audit Logs By Entity
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetByEntityAsync(
        string entityName,
        Guid? entityId = null,
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetByEntityAsync(
                entityName,
                entityId,
                cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Audit Logs By Action
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetByActionAsync(
        AuditAction action,
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetByActionAsync(
                action,
                cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Audit Logs By Severity
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetBySeverityAsync(
        AuditSeverity severity,
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetBySeverityAsync(
                severity,
                cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Audit Logs By Request ID
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetByRequestIdAsync(
        string requestId,
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetByRequestIdAsync(
                requestId,
                cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Audit Logs Within Date Range
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>>
        GetByDateRangeAsync(
            DateTime from,
            DateTime to,
            CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetByDateRangeAsync(
                from,
                to,
                cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Failed Audit Logs
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetFailedAsync(
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetFailedAsync(cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Critical Audit Logs
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetCriticalAsync(
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetCriticalAsync(cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Recent Audit Logs
    // ================================================================

    public async Task<IReadOnlyList<AuditLogResponseDto>> GetRecentAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await _unitOfWork.AuditLogs
            .GetRecentAsync(
                count,
                cancellationToken);

        return auditLogs
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Count Audit Logs By User
    // ================================================================

    public async Task<int> CountByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.AuditLogs
            .CountByUserIdAsync(
                userId,
                cancellationToken);
    }


    // ================================================================
    // Count Failed Audit Logs
    // ================================================================

    public async Task<int> CountFailedAsync(
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.AuditLogs
            .CountFailedAsync(cancellationToken);
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static AuditLogResponseDto MapToResponse(
        AuditLog auditLog)
    {
        return new AuditLogResponseDto
        {
            Id = auditLog.Id,

            UserId = auditLog.UserId,

            UserName = auditLog.User?.FullName,

            Action = auditLog.Action,

            EntityName = auditLog.EntityName,

            EntityId = auditLog.EntityId,

            Description = auditLog.Description,

            Severity = auditLog.Severity,

            IsSuccessful = auditLog.IsSuccessful,

            Timestamp = auditLog.Timestamp
        };
    }


    private static AuditLogDetailResponseDto MapToDetailResponse(
        AuditLog auditLog)
    {
        return new AuditLogDetailResponseDto
        {
            Id = auditLog.Id,

            UserId = auditLog.UserId,

            UserName = auditLog.User?.FullName,

            Action = auditLog.Action,

            EntityName = auditLog.EntityName,

            EntityId = auditLog.EntityId,

            Description = auditLog.Description,

            OldValues = auditLog.OldValues,

            NewValues = auditLog.NewValues,

            ChangedProperties = auditLog.ChangedProperties,

            IpAddress = auditLog.IpAddress,

            UserAgent = auditLog.UserAgent,

            RequestId = auditLog.RequestId,

            Severity = auditLog.Severity,

            Timestamp = auditLog.Timestamp,

            IsSuccessful = auditLog.IsSuccessful,

            ErrorMessage = auditLog.ErrorMessage,

            CreatedAt = auditLog.CreatedAt,

            CreatedBy = auditLog.CreatedBy,

            UpdatedAt = auditLog.UpdatedAt,

            UpdatedBy = auditLog.UpdatedBy
        };
    }
}