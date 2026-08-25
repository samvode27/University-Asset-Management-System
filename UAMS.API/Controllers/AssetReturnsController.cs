using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.AssetReturns.Requests;
using UAMS.Application.DTOs.AssetReturns.Responses;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Enums;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AssetReturnsController : ControllerBase
{
    private readonly IAssetReturnService _assetReturnService;

    public AssetReturnsController(
        IAssetReturnService assetReturnService)
    {
        _assetReturnService = assetReturnService
            ?? throw new ArgumentNullException(nameof(assetReturnService));
    }


    // ================================================================
    // Create
    // POST: api/AssetReturns
    // ================================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetReturnResponseDto>> Create(
        [FromBody] CreateAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _assetReturnService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
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
    }


    // ================================================================
    // Get By Id
    // GET: api/AssetReturns/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(AssetReturnDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetReturnDetailResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetReturnService.GetByIdAsync(
            id,
            cancellationToken);

        if (result is null)
        {
            return NotFound(new
            {
                message = $"Asset return with ID '{id}' was not found."
            });
        }

        return Ok(result);
    }


    // ================================================================
    // Get By Return Number
    // GET: api/AssetReturns/number/{returnNumber}
    // ================================================================

    [HttpGet("number/{returnNumber}")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetReturnResponseDto>>
        GetByReturnNumber(
            string returnNumber,
            CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(returnNumber))
        {
            return BadRequest(new
            {
                message = "Return number is required."
            });
        }

        var result =
            await _assetReturnService.GetByReturnNumberAsync(
                returnNumber,
                cancellationToken);

        if (result is null)
        {
            return NotFound(new
            {
                message =
                    $"Asset return with number '{returnNumber}' was not found."
            });
        }

        return Ok(result);
    }


    // ================================================================
    // Get By Asset
    // GET: api/AssetReturns/asset/{assetId}
    // ================================================================

    [HttpGet("asset/{assetId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
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
    // GET: api/AssetReturns/assignment/{assetAssignmentId}
    // ================================================================

    [HttpGet("assignment/{assetAssignmentId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
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
    // GET: api/AssetReturns/employee/{employeeId}
    // ================================================================

    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
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
    // GET: api/AssetReturns/received-by/{receivedById}
    // ================================================================

    [HttpGet("received-by/{receivedById:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
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
    // GET: api/AssetReturns/inspected-by/{inspectedById}
    // ================================================================

    [HttpGet("inspected-by/{inspectedById:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
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
    // GET: api/AssetReturns/status/{status}
    // ================================================================

    [HttpGet("status/{status}")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetByStatus(
            AssetReturnStatus status,
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetByStatusAsync(
                status,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get Pending Inspection
    // GET: api/AssetReturns/pending-inspection
    // ================================================================

    [HttpGet("pending-inspection")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetPendingInspection(
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetPendingInspectionAsync(
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get With Damage
    // GET: api/AssetReturns/with-damage
    // ================================================================

    [HttpGet("with-damage")]
    [ProducesResponseType(
        typeof(IReadOnlyList<AssetReturnResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AssetReturnResponseDto>>>
        GetWithDamage(
            CancellationToken cancellationToken)
    {
        var result =
            await _assetReturnService.GetWithDamageAsync(
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Filter / Search / Pagination
    // GET: api/AssetReturns
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(AssetReturnListResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AssetReturnListResponseDto>> Filter(
        [FromQuery] AssetReturnFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetReturnService.FilterAsync(
                    request,
                    cancellationToken);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // Update
    // PUT: api/AssetReturns/{id}
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetReturnResponseDto>> Update(
        Guid id,
        [FromBody] UpdateAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetReturnService.UpdateAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
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
    }


    // ================================================================
    // Inspect
    // POST: api/AssetReturns/{id}/inspect
    // ================================================================

    [HttpPost("{id:guid}/inspect")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetReturnResponseDto>> Inspect(
        Guid id,
        [FromBody] InspectAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetReturnService.InspectAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
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
    }


    // ================================================================
    // Complete
    // POST: api/AssetReturns/{id}/complete
    // ================================================================

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetReturnResponseDto>> Complete(
        Guid id,
        [FromBody] CompleteAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetReturnService.CompleteAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
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
    }


    // ================================================================
    // Cancel
    // POST: api/AssetReturns/{id}/cancel
    // ================================================================

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(
        typeof(AssetReturnResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetReturnResponseDto>> Cancel(
        Guid id,
        [FromBody] CancelAssetReturnRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetReturnService.CancelAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
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
    }
}

