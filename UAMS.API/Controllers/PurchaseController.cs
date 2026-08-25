using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Purchases.Requests;
using UAMS.Application.DTOs.Purchases.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService
            ?? throw new ArgumentNullException(nameof(purchaseService));
    }


    // ================================================================
    // GET: api/purchase/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(PurchaseResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PurchaseResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _purchaseService.GetByIdAsync(
                id,
                cancellationToken);

            return Ok(response);
        }
        catch (ArgumentException ex)
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
    }


    // ================================================================
    // GET: api/purchase/{id}/details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(PurchaseDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PurchaseDetailResponseDto>> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _purchaseService.GetDetailsAsync(
                id,
                cancellationToken);

            return Ok(response);
        }
        catch (ArgumentException ex)
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
    }


    // ================================================================
    // GET: api/purchase
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(PurchaseListResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<PurchaseListResponseDto>> GetAll(
        [FromQuery] PurchaseFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await _purchaseService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(response);
    }


    // ================================================================
    // POST: api/purchase
    // ================================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(PurchaseResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PurchaseResponseDto>> Create(
        [FromBody] CreatePurchaseRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _purchaseService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
        }
        catch (ArgumentException ex)
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
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // PUT: api/purchase/{id}
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(PurchaseResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PurchaseResponseDto>> Update(
        Guid id,
        [FromBody] UpdatePurchaseRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _purchaseService.UpdateAsync(
                id,
                request,
                cancellationToken);

            return Ok(response);
        }
        catch (ArgumentException ex)
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
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // DELETE: api/purchase/{id}
    // ================================================================

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _purchaseService.DeleteAsync(
                id,
                cancellationToken);

            return NoContent();
        }
        catch (ArgumentException ex)
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
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }
}