using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UAMS.Application.DTOs.AssetTransfers.Requests;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Enums;

namespace UAMS.API.Controllers.AssetTransfers;

[ApiController]
[Route("api/asset-transfers")]
[Authorize]
public class AssetTransferController : ControllerBase
{
    private readonly IAssetTransferService _assetTransferService;

    public AssetTransferController(
        IAssetTransferService assetTransferService)
    {
        _assetTransferService = assetTransferService
            ?? throw new ArgumentNullException(nameof(assetTransferService));
    }


    // ================================================================
    // Create
    // ================================================================

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssetTransferRequestDto request,
        CancellationToken cancellationToken)
    {
        var requestedById = GetCurrentUserId();

        var result = await _assetTransferService.CreateAsync(
            request,
            requestedById,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // ================================================================
    // Get By Id
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService.GetByIdAsync(
            id,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    // ================================================================
    // Get By Transfer Number
    // ================================================================

    [HttpGet("number/{transferNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTransferNumber(
        string transferNumber,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService
            .GetByTransferNumberAsync(
                transferNumber,
                cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    // ================================================================
    // Get By Asset
    // ================================================================

    [HttpGet("asset/{assetId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByAssetId(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService.GetByAssetIdAsync(
            assetId,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Asset Assignment
    // ================================================================

    [HttpGet("assignment/{assetAssignmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByAssetAssignmentId(
        Guid assetAssignmentId,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService
            .GetByAssetAssignmentIdAsync(
                assetAssignmentId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Requested By
    // ================================================================

    [HttpGet("requested-by/{requestedById:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByRequestedById(
        Guid requestedById,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService
            .GetByRequestedByIdAsync(
                requestedById,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By From Employee
    // ================================================================

    [HttpGet("from-employee/{fromEmployeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByFromEmployeeId(
        Guid fromEmployeeId,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService
            .GetByFromEmployeeIdAsync(
                fromEmployeeId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By To Employee
    // ================================================================

    [HttpGet("to-employee/{toEmployeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByToEmployeeId(
        Guid toEmployeeId,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService
            .GetByToEmployeeIdAsync(
                toEmployeeId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By From Department
    // ================================================================

    [HttpGet("from-department/{fromDepartmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByFromDepartmentId(
        Guid fromDepartmentId,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService
            .GetByFromDepartmentIdAsync(
                fromDepartmentId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By To Department
    // ================================================================

    [HttpGet("to-department/{toDepartmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByToDepartmentId(
        Guid toDepartmentId,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService
            .GetByToDepartmentIdAsync(
                toDepartmentId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Status
    // ================================================================

    [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByStatus(
        AssetTransferStatus status,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(status))
        {
            return BadRequest("Invalid asset transfer status.");
        }

        var result = await _assetTransferService.GetByStatusAsync(
            status,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get Pending
    // ================================================================

    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPending(
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService.GetPendingAsync(
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Update
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAssetTransferRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Approve
    // ================================================================

    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Approve(
        Guid id,
        [FromBody] ApproveAssetTransferRequestDto request,
        CancellationToken cancellationToken)
    {
        var approvedById = GetCurrentUserId();

        var result = await _assetTransferService.ApproveAsync(
            id,
            request,
            approvedById,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Reject
    // ================================================================

    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reject(
        Guid id,
        [FromBody] RejectAssetTransferRequestDto request,
        CancellationToken cancellationToken)
    {
        var approvedById = GetCurrentUserId();

        var result = await _assetTransferService.RejectAsync(
            id,
            request,
            approvedById,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Complete
    // ================================================================

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromBody] CompleteAssetTransferRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService.CompleteAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Cancel
    // ================================================================

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromQuery] string? reason,
        CancellationToken cancellationToken)
    {
        var result = await _assetTransferService.CancelAsync(
            id,
            reason,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Current User
    // ================================================================

    private Guid GetCurrentUserId()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException(
                "Authenticated user ID is missing or invalid.");
        }

        return userId;
    }
}