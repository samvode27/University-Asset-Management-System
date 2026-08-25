using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UAMS.Application.DTOs.AssetAssignments.Requests;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Enums;

namespace UAMS.API.Controllers.AssetAssignments;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssetAssignmentController : ControllerBase
{
    private readonly IAssetAssignmentService _assetAssignmentService;

    public AssetAssignmentController(
        IAssetAssignmentService assetAssignmentService)
    {
        _assetAssignmentService =
            assetAssignmentService
            ?? throw new ArgumentNullException(
                nameof(assetAssignmentService));
    }


    // ================================================================
    // Create
    // ================================================================

    /// <summary>
    /// Creates a new asset assignment.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssetAssignmentRequestDto request,
        CancellationToken cancellationToken)
    {
        var assignedById = GetCurrentUserId();

        if (assignedById == null)
        {
            return Unauthorized();
        }

        var result =
            await _assetAssignmentService.CreateAsync(
                request,
                assignedById.Value,
                cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // ================================================================
    // Get By Id
    // ================================================================

    /// <summary>
    /// Gets an asset assignment by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService.GetByIdAsync(
                id,
                cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    // ================================================================
    // Get By Asset
    // ================================================================

    /// <summary>
    /// Gets all assignments associated with an asset.
    /// </summary>
    [HttpGet("asset/{assetId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByAssetId(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService.GetByAssetIdAsync(
                assetId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Employee
    // ================================================================

    /// <summary>
    /// Gets all assignments associated with an employee.
    /// </summary>
    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByEmployeeId(
        Guid employeeId,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService.GetByEmployeeIdAsync(
                employeeId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Asset Request
    // ================================================================

    /// <summary>
    /// Gets the assignment associated with an asset request.
    /// </summary>
    [HttpGet("request/{assetRequestId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAssetRequestId(
        Guid assetRequestId,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService
                .GetByAssetRequestIdAsync(
                    assetRequestId,
                    cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    // ================================================================
    // Get Active By Asset
    // ================================================================

    /// <summary>
    /// Gets the currently active assignment for an asset.
    /// </summary>
    [HttpGet("asset/{assetId:guid}/active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveByAssetId(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService
                .GetActiveByAssetIdAsync(
                    assetId,
                    cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    // ================================================================
    // Get Active By Employee
    // ================================================================

    /// <summary>
    /// Gets all currently active assignments for an employee.
    /// </summary>
    [HttpGet("employee/{employeeId:guid}/active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetActiveByEmployeeId(
        Guid employeeId,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService
                .GetActiveByEmployeeIdAsync(
                    employeeId,
                    cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Status
    // ================================================================

    /// <summary>
    /// Gets asset assignments by status.
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByStatus(
        AssetAssignmentStatus status,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(status))
        {
            return BadRequest(
                new
                {
                    message = "Invalid asset assignment status."
                });
        }

        var result =
            await _assetAssignmentService.GetByStatusAsync(
                status,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Update
    // ================================================================

    /// <summary>
    /// Updates an existing asset assignment.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAssetAssignmentRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService.UpdateAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Complete / Return
    // ================================================================

    /// <summary>
    /// Completes an asset assignment and records its return.
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromBody] CompleteAssetAssignmentRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService.CompleteAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Cancel
    // ================================================================

    /// <summary>
    /// Cancels an asset assignment.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromBody] CancelAssetAssignmentRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetAssignmentService.CancelAsync(
                id,
                request.Reason,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Current User
    // ================================================================

    private Guid? GetCurrentUserId()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return null;
        }

        return Guid.TryParse(
            userIdClaim,
            out var userId)
            ? userId
            : null;
    }
}

