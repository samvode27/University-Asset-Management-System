using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UAMS.Application.DTOs.AssetRequests.Requests;
using UAMS.Application.DTOs.AssetRequests.Responses;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Enums;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssetRequestController : ControllerBase
{
    private readonly IAssetRequestService _assetRequestService;

    public AssetRequestController(
        IAssetRequestService assetRequestService)
    {
        _assetRequestService =
            assetRequestService
            ?? throw new ArgumentNullException(
                nameof(assetRequestService));
    }


    // ================================================================
    // GET: api/AssetRequest/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(AssetRequestResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetRequestResponseDto>>
        GetById(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService.GetByIdAsync(
                    id,
                    cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/AssetRequest/{id}/details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(AssetRequestDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetRequestDetailResponseDto>>
        GetDetails(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService.GetDetailsAsync(
                    id,
                    cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/AssetRequest/by-number/{requestNumber}
    // ================================================================

    [HttpGet("by-number/{requestNumber}")]
    [ProducesResponseType(
        typeof(AssetRequestResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetRequestResponseDto>>
        GetByRequestNumber(
            string requestNumber,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService
                    .GetByRequestNumberAsync(
                        requestNumber,
                        cancellationToken);

            return Ok(result);
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
    // GET: api/AssetRequest/by-requester/{requesterId}
    // ================================================================

    [HttpGet("by-requester/{requesterId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetRequestResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        IReadOnlyList<AssetRequestResponseDto>>>
        GetByRequesterId(
            Guid requesterId,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService
                    .GetByRequesterIdAsync(
                        requesterId,
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
    }


    // ================================================================
    // GET: api/AssetRequest/by-asset/{assetId}
    // ================================================================

    [HttpGet("by-asset/{assetId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetRequestResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        IReadOnlyList<AssetRequestResponseDto>>>
        GetByAssetId(
            Guid assetId,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService
                    .GetByAssetIdAsync(
                        assetId,
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
    }


    // ================================================================
    // GET: api/AssetRequest/by-department/{departmentId}
    // ================================================================

    [HttpGet("by-department/{departmentId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetRequestResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        IReadOnlyList<AssetRequestResponseDto>>>
        GetByDepartmentId(
            Guid departmentId,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService
                    .GetByDepartmentIdAsync(
                        departmentId,
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
    }


    // ================================================================
    // GET: api/AssetRequest/by-status/{status}
    // ================================================================

    [HttpGet("by-status/{status}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetRequestResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        IReadOnlyList<AssetRequestResponseDto>>>
        GetByStatus(
            AssetRequestStatus status,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService
                    .GetByStatusAsync(
                        status,
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
    }


    // ================================================================
    // GET: api/AssetRequest/by-requester/{requesterId}/status/{status}
    // ================================================================

    [HttpGet(
        "by-requester/{requesterId:guid}/status/{status}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetRequestResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        IReadOnlyList<AssetRequestResponseDto>>>
        GetByRequesterAndStatus(
            Guid requesterId,
            AssetRequestStatus status,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService
                    .GetByRequesterAndStatusAsync(
                        requesterId,
                        status,
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
    }


    // ================================================================
    // GET: api/AssetRequest
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(AssetRequestListResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<AssetRequestListResponseDto>>
        GetAll(
            [FromQuery] AssetRequestFilterRequestDto request,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetRequestService.GetAllAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // POST: api/AssetRequest
    // ================================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(AssetRequestResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetRequestResponseDto>>
        Create(
            [FromBody] CreateAssetRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var requesterId =
                GetCurrentUserId();

            var result =
                await _assetRequestService.CreateAsync(
                    request,
                    requesterId,
                    cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = result.Id
                },
                result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
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
    // PUT: api/AssetRequest/{id}
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(AssetRequestResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetRequestResponseDto>>
        Update(
            Guid id,
            [FromBody] UpdateAssetRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetRequestService.UpdateAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
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
    // PATCH: api/AssetRequest/{id}/department-head-review
    // ================================================================

    [HttpPatch("{id:guid}/department-head-review")]
    [ProducesResponseType(
        typeof(AssetRequestApprovalResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<
        AssetRequestApprovalResponseDto>>
        ReviewByDepartmentHead(
            Guid id,
            [FromBody] DepartmentHeadReviewRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var departmentHeadId =
                GetCurrentUserId();

            var result =
                await _assetRequestService
                    .ReviewByDepartmentHeadAsync(
                        id,
                        request,
                        departmentHeadId,
                        cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
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
    // PATCH: api/AssetRequest/{id}/asset-manager-review
    // ================================================================

    [HttpPatch("{id:guid}/asset-manager-review")]
    [ProducesResponseType(
        typeof(AssetRequestApprovalResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<
        AssetRequestApprovalResponseDto>>
        ReviewByAssetManager(
            Guid id,
            [FromBody] AssetManagerReviewRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var assetManagerId =
                GetCurrentUserId();

            var result =
                await _assetRequestService
                    .ReviewByAssetManagerAsync(
                        id,
                        request,
                        assetManagerId,
                        cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
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
    // PATCH: api/AssetRequest/{id}/cancel
    // ================================================================

    [HttpPatch("{id:guid}/cancel")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromBody] CancelAssetRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _assetRequestService.CancelAsync(
                id,
                request,
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
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
    // Current Authenticated User ID
    // ================================================================

    private Guid GetCurrentUserId()
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!Guid.TryParse(
                userIdClaim,
                out var userId))
        {
            throw new UnauthorizedAccessException(
                "Authenticated user ID is missing or invalid.");
        }

        return userId;
    }
}

