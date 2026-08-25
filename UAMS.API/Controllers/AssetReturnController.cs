using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.AssetReturns.Requests;
using UAMS.Application.DTOs.AssetReturns.Responses;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Enums;

namespace UAMS.API.Controllers.AssetReturns;

[ApiController]
[Route("api/asset-returns")]
[Authorize]
public class AssetReturnController : ControllerBase
{
    private readonly IAssetReturnService _assetReturnService;

    public AssetReturnController(
        IAssetReturnService assetReturnService)
    {
        _assetReturnService = assetReturnService
            ?? throw new ArgumentNullException(
                nameof(assetReturnService));
    }


    // ================================================================
    // Create
    // POST: api/asset-returns
    // ================================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AssetReturnResponseDto>> Create(
        [FromBody] CreateAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetReturnService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // ================================================================
    // Get By Id
    // GET: api/asset-returns/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(AssetReturnDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssetReturnDetailResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetReturnService.GetByIdAsync(
            id,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    // ================================================================
    // Get By Return Number
    // GET: api/asset-returns/number/{returnNumber}
    // ================================================================

    [HttpGet("number/{returnNumber}")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssetReturnResponseDto>>
        GetByReturnNumber(
            string returnNumber,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetByReturnNumberAsync(
                returnNumber,
                cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    // ================================================================
    // Get By Asset
    // GET: api/asset-returns/asset/{assetId}
    // ================================================================

    [HttpGet("asset/{assetId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<
        ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetByAssetId(
            Guid assetId,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetByAssetIdAsync(
                assetId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Asset Assignment
    // GET: api/asset-returns/assignment/{assetAssignmentId}
    // ================================================================

    [HttpGet("assignment/{assetAssignmentId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<
        ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetByAssetAssignmentId(
            Guid assetAssignmentId,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetByAssetAssignmentIdAsync(
                assetAssignmentId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Employee
    // GET: api/asset-returns/employee/{employeeId}
    // ================================================================

    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<
        ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetByEmployeeId(
            Guid employeeId,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetByEmployeeIdAsync(
                employeeId,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Received By
    // GET: api/asset-returns/received-by/{receivedById}
    // ================================================================

    [HttpGet("received-by/{receivedById:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<
        ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetByReceivedById(
            Guid receivedById,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetByReceivedByIdAsync(
                receivedById,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Inspector
    // GET: api/asset-returns/inspected-by/{inspectedById}
    // ================================================================

    [HttpGet("inspected-by/{inspectedById:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<
        ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetByInspectedById(
            Guid inspectedById,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetByInspectedByIdAsync(
                inspectedById,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get By Status
    // GET: api/asset-returns/status/{status}
    // ================================================================

    [HttpGet("status/{status}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<
        ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetByStatus(
            AssetReturnStatus status,
            CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(status))
        {
            return BadRequest(
                new
                {
                    message = "Invalid asset return status."
                });
        }

        var result =
            await _assetReturnService.GetByStatusAsync(
                status,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get Pending Inspection
    // GET: api/asset-returns/pending-inspection
    // ================================================================

    [HttpGet("pending-inspection")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<
        ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetPendingInspection(
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetPendingInspectionAsync(
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get Returns With Damage
    // GET: api/asset-returns/with-damage
    // ================================================================

    [HttpGet("with-damage")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<
        ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetWithDamage(
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetWithDamageAsync(
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Filter
    // POST: api/asset-returns/filter
    // ================================================================

    [HttpPost("filter")]
    [ProducesResponseType(
        typeof(AssetReturnListResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssetReturnListResponseDto>>
        Filter(
            [FromBody] AssetReturnFilterRequestDto request,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.FilterAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Update
    // PUT: api/asset-returns/{id}
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssetReturnResponseDto>> Update(
        Guid id,
        [FromBody] UpdateAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.UpdateAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Inspect
    // POST: api/asset-returns/{id}/inspect
    // ================================================================

    [HttpPost("{id:guid}/inspect")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssetReturnResponseDto>> Inspect(
        Guid id,
        [FromBody] InspectAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.InspectAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Complete
    // POST: api/asset-returns/{id}/complete
    // ================================================================

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssetReturnResponseDto>> Complete(
        Guid id,
        [FromBody] CompleteAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.CompleteAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Cancel
    // POST: api/asset-returns/{id}/cancel
    // ================================================================

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssetReturnResponseDto>> Cancel(
        Guid id,
        [FromBody] CancelAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.CancelAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }
}