using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UAMS.Application.DTOs.AssetDisposals.Requests;
using UAMS.Application.DTOs.AssetDisposals.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class AssetDisposalsController : ControllerBase
{
    private readonly IAssetDisposalService _assetDisposalService;

    public AssetDisposalsController(
        IAssetDisposalService assetDisposalService)
    {
        _assetDisposalService = assetDisposalService;
    }


    // ================================================================
    // GET: api/AssetDisposals/{id}
    // Get Disposal By ID
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(AssetDisposalResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetDisposalResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var disposal =
                await _assetDisposalService.GetByIdAsync(
                    id,
                    cancellationToken);

            if (disposal is null)
            {
                return NotFound(
                    new
                    {
                        message = "Asset disposal record was not found."
                    });
            }

            return Ok(disposal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ================================================================
    // GET: api/AssetDisposals/{id}/details
    // Get Disposal Details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(AssetDisposalDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetDisposalDetailResponseDto>> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var disposal =
                await _assetDisposalService.GetDetailsAsync(
                    id,
                    cancellationToken);

            if (disposal is null)
            {
                return NotFound(
                    new
                    {
                        message = "Asset disposal record was not found."
                    });
            }

            return Ok(disposal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ================================================================
    // GET: api/AssetDisposals/number/{disposalNumber}
    // Get Disposal By Disposal Number
    // ================================================================

    [HttpGet("number/{disposalNumber}")]
    [ProducesResponseType(
        typeof(AssetDisposalResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetDisposalResponseDto>>
        GetByDisposalNumber(
            string disposalNumber,
            CancellationToken cancellationToken)
    {
        try
        {
            var disposal =
                await _assetDisposalService.GetByDisposalNumberAsync(
                    disposalNumber,
                    cancellationToken);

            if (disposal is null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Asset disposal record was not found."
                    });
            }

            return Ok(disposal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ================================================================
    // GET: api/AssetDisposals
    // Get Disposal Records With Filtering / Pagination
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(AssetDisposalListResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<AssetDisposalListResponseDto>> GetAll(
        [FromQuery] AssetDisposalFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _assetDisposalService.GetAllAsync(
                    request,
                    cancellationToken);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ================================================================
    // POST: api/AssetDisposals
    // Create Disposal Request
    // ================================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(AssetDisposalResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AssetDisposalResponseDto>> Create(
        [FromBody] CreateAssetDisposalRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var requestedById = GetCurrentUserId();

            var disposal =
                await _assetDisposalService.CreateAsync(
                    request,
                    requestedById,
                    cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = disposal.Id
                },
                disposal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ================================================================
    // PUT: api/AssetDisposals/{id}
    // Update Disposal Request
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(AssetDisposalResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AssetDisposalResponseDto>> Update(
        Guid id,
        [FromBody] UpdateAssetDisposalRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var disposal =
                await _assetDisposalService.UpdateAsync(
                    id,
                    request,
                    cancellationToken);

            if (disposal is null)
            {
                return NotFound(
                    new
                    {
                        message = "Asset disposal record was not found."
                    });
            }

            return Ok(disposal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                new
                {
                    message = ex.Message
                });
        }
    }


// ================================================================
// POST: api/AssetDisposals/{id}/review
// Start Disposal Review
// ================================================================

[HttpPost("{id:guid}/review")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public async Task<IActionResult> StartReview(
    Guid id,
    CancellationToken cancellationToken)
{
    try
    {
        await _assetDisposalService.StartReviewAsync(
            id,
            cancellationToken);

        return Ok(new
        {
            message = "Asset disposal moved to under review successfully."
        });
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
    // POST: api/AssetDisposals/{id}/approve
    // Approve Disposal Request
    // ================================================================

    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(
        typeof(AssetDisposalResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AssetDisposalResponseDto>>
        Approve(
            Guid id,
            [FromBody] ApproveAssetDisposalRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var approvedById = GetCurrentUserId();

            var disposal =
                await _assetDisposalService.ApproveAsync(
                    id,
                    request,
                    approvedById,
                    cancellationToken);

            if (disposal is null)
            {
                return NotFound(
                    new
                    {
                        message = "Asset disposal record was not found."
                    });
            }

            return Ok(disposal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ================================================================
    // POST: api/AssetDisposals/{id}/reject
    // Reject Disposal Request
    // ================================================================

    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(
        typeof(AssetDisposalResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AssetDisposalResponseDto>>
        Reject(
            Guid id,
            [FromBody] RejectAssetDisposalRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var disposal =
                await _assetDisposalService.RejectAsync(
                    id,
                    request,
                    cancellationToken);

            if (disposal is null)
            {
                return NotFound(
                    new
                    {
                        message = "Asset disposal record was not found."
                    });
            }

            return Ok(disposal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ================================================================
    // POST: api/AssetDisposals/{id}/complete
    // Complete Disposal
    // ================================================================

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(
        typeof(AssetDisposalResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AssetDisposalResponseDto>>
        Complete(
            Guid id,
            [FromBody] CompleteAssetDisposalRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var completedById = GetCurrentUserId();

            var disposal =
                await _assetDisposalService.CompleteAsync(
                    id,
                    request,
                    completedById,
                    cancellationToken);

            if (disposal is null)
            {
                return NotFound(
                    new
                    {
                        message = "Asset disposal record was not found."
                    });
            }

            return Ok(disposal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                new
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

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            throw new UnauthorizedAccessException(
                "The authenticated user identifier is missing or invalid.");
        }

        return userId;
    }
}