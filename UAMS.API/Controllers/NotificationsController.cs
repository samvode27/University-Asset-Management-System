using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UAMS.Application.DTOs.Notifications.Requests;
using UAMS.Application.DTOs.Notifications.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(
        INotificationService notificationService)
    {
        _notificationService = notificationService;
    }


    // ================================================================
    // GET: api/Notifications/{id}
    // Get Notification By ID
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(NotificationResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotificationResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();

            var notification =
                await _notificationService.GetByIdAsync(
                    id,
                    userId,
                    cancellationToken);

            return Ok(notification);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/Notifications/{id}/details
    // Get Notification Details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(NotificationDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotificationDetailResponseDto>>
        GetDetails(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();

            var notification =
                await _notificationService.GetDetailsAsync(
                    id,
                    userId,
                    cancellationToken);

            return Ok(notification);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/Notifications
    // Get Notifications With Filtering / Pagination
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(NotificationListResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<NotificationListResponseDto>> GetAll(
        [FromQuery] NotificationFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();

            var result =
                await _notificationService.GetAllAsync(
                    request,
                    userId,
                    cancellationToken);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }


    // ================================================================
    // GET: api/Notifications/my
    // Get Current User Notifications
    // ================================================================

    [HttpGet("my")]
    [ProducesResponseType(
        typeof(IReadOnlyList<NotificationResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotificationResponseDto>>>
        GetMyNotifications(
            CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var notifications =
            await _notificationService.GetByUserIdAsync(
                userId,
                cancellationToken);

        return Ok(notifications);
    }


    // ================================================================
    // GET: api/Notifications/unread
    // Get Unread Notifications
    // ================================================================

    [HttpGet("unread")]
    [ProducesResponseType(
        typeof(IReadOnlyList<NotificationResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotificationResponseDto>>>
        GetUnread(
            CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var notifications =
            await _notificationService.GetUnreadAsync(
                userId,
                cancellationToken);

        return Ok(notifications);
    }


    // ================================================================
    // GET: api/Notifications/read
    // Get Read Notifications
    // ================================================================

    [HttpGet("read")]
    [ProducesResponseType(
        typeof(IReadOnlyList<NotificationResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotificationResponseDto>>>
        GetRead(
            CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var notifications =
            await _notificationService.GetReadAsync(
                userId,
                cancellationToken);

        return Ok(notifications);
    }


    // ================================================================
    // GET: api/Notifications/high-priority
    // Get High Priority Notifications
    // ================================================================

    [HttpGet("high-priority")]
    [ProducesResponseType(
        typeof(IReadOnlyList<NotificationResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotificationResponseDto>>>
        GetHighPriority(
            CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var notifications =
            await _notificationService.GetHighPriorityAsync(
                userId,
                cancellationToken);

        return Ok(notifications);
    }


    // ================================================================
    // GET: api/Notifications/active
    // Get Active Notifications
    // ================================================================

    [HttpGet("active")]
    [ProducesResponseType(
        typeof(IReadOnlyList<NotificationResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotificationResponseDto>>>
        GetActive(
            CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var notifications =
            await _notificationService.GetActiveAsync(
                userId,
                cancellationToken);

        return Ok(notifications);
    }


    // ================================================================
    // GET: api/Notifications/unread-count
    // Get Unread Notification Count
    // ================================================================

    [HttpGet("unread-count")]
    [ProducesResponseType(
        typeof(int),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetUnreadCount(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var count =
            await _notificationService.GetUnreadCountAsync(
                userId,
                cancellationToken);

        return Ok(count);
    }


    // ================================================================
    // GET: api/Notifications/reference/{referenceId}
    // Get Notifications By Reference
    // ================================================================

    [HttpGet("reference/{referenceId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<NotificationResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotificationResponseDto>>>
        GetByReference(
            Guid referenceId,
            [FromQuery] string referenceType,
            CancellationToken cancellationToken)
    {
        try
        {
            var notifications =
                await _notificationService.GetByReferenceAsync(
                    referenceId,
                    referenceType,
                    cancellationToken);

            return Ok(notifications);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // POST: api/Notifications
    // Create Notification
    // ================================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(NotificationResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotificationResponseDto>> Create(
        [FromBody] CreateNotificationRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var notification =
                await _notificationService.CreateAsync(
                    request,
                    cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = notification.Id
                },
                notification);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // POST: api/Notifications/read
    // Mark Notification As Read
    // ================================================================

    [HttpPost("read")]
    [ProducesResponseType(
        typeof(NotificationResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<NotificationResponseDto>>
        MarkAsRead(
            [FromBody] MarkNotificationAsReadRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();

            var notification =
                await _notificationService.MarkAsReadAsync(
                    request,
                    userId,
                    cancellationToken);

            return Ok(notification);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // Current Authenticated User
    // ================================================================

    private Guid GetCurrentUserId()
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? User.FindFirstValue("userId");

        if (!Guid.TryParse(
                userIdValue,
                out var userId))
        {
            throw new UnauthorizedAccessException(
                "The authenticated user identifier is missing or invalid.");
        }

        return userId;
    }
}