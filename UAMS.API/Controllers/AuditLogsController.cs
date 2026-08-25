using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.AuditLogs.Requests;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Enums;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(
        IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }


    // ================================================================
    // GET: api/AuditLogs
    // ================================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] AuditLogFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _auditLogService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _auditLogService.GetByIdAsync(
            id,
            cancellationToken);

        if (result is null)
        {
            return NotFound(new
            {
                message = "Audit log not found."
            });
        }

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/user/{userId}
    // ================================================================

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _auditLogService.GetByUserIdAsync(
            userId,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/entity/{entityName}
    // ================================================================

    [HttpGet("entity/{entityName}")]
    public async Task<IActionResult> GetByEntity(
        string entityName,
        [FromQuery] Guid? entityId,
        CancellationToken cancellationToken)
    {
        var result = await _auditLogService.GetByEntityAsync(
            entityName,
            entityId,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/action/{action}
    // ================================================================

    [HttpGet("action/{action}")]
    public async Task<IActionResult> GetByAction(
        AuditAction action,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(action))
        {
            return BadRequest(new
            {
                message = "Invalid audit action."
            });
        }

        var result = await _auditLogService.GetByActionAsync(
            action,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/severity/{severity}
    // ================================================================

    [HttpGet("severity/{severity}")]
    public async Task<IActionResult> GetBySeverity(
        AuditSeverity severity,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(severity))
        {
            return BadRequest(new
            {
                message = "Invalid audit severity."
            });
        }

        var result = await _auditLogService.GetBySeverityAsync(
            severity,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/request/{requestId}
    // ================================================================

    [HttpGet("request/{requestId}")]
    public async Task<IActionResult> GetByRequestId(
        string requestId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(requestId))
        {
            return BadRequest(new
            {
                message = "Request ID is required."
            });
        }

        var result = await _auditLogService.GetByRequestIdAsync(
            requestId,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/date-range
    // ================================================================

    [HttpGet("date-range")]
    public async Task<IActionResult> GetByDateRange(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken cancellationToken)
    {
        if (from > to)
        {
            return BadRequest(new
            {
                message =
                    "From date must be earlier than or equal to To date."
            });
        }

        var result = await _auditLogService.GetByDateRangeAsync(
            from,
            to,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/failed
    // ================================================================

    [HttpGet("failed")]
    public async Task<IActionResult> GetFailed(
        CancellationToken cancellationToken)
    {
        var result = await _auditLogService.GetFailedAsync(
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/critical
    // ================================================================

    [HttpGet("critical")]
    public async Task<IActionResult> GetCritical(
        CancellationToken cancellationToken)
    {
        var result = await _auditLogService.GetCriticalAsync(
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/recent
    // ================================================================

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecent(
        [FromQuery] int count = 20,
        CancellationToken cancellationToken = default)
    {
        if (count < 1 || count > 100)
        {
            return BadRequest(new
            {
                message = "Count must be between 1 and 100."
            });
        }

        var result = await _auditLogService.GetRecentAsync(
            count,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AuditLogs/user/{userId}/count
    // ================================================================

    [HttpGet("user/{userId:guid}/count")]
    public async Task<IActionResult> CountByUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _auditLogService.CountByUserIdAsync(
            userId,
            cancellationToken);

        return Ok(new
        {
            userId,
            count = result
        });
    }


    // ================================================================
    // GET: api/AuditLogs/failed/count
    // ================================================================

    [HttpGet("failed/count")]
    public async Task<IActionResult> CountFailed(
        CancellationToken cancellationToken)
    {
        var result = await _auditLogService.CountFailedAsync(
            cancellationToken);

        return Ok(new
        {
            count = result
        });
    }
}