using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Barcode.Requests;
using UAMS.Application.DTOs.Barcode.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BarcodeController : ControllerBase
{
    private readonly IBarcodeService _barcodeService;

    public BarcodeController(IBarcodeService barcodeService)
    {
        _barcodeService = barcodeService
            ?? throw new ArgumentNullException(nameof(barcodeService));
    }


    // ================================================================
    // GET: api/barcode/{id}
    // Get Barcode By ID
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(BarcodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BarcodeResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _barcodeService.GetByIdAsync(
                id,
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/barcode/{id}/details
    // Get Barcode Details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(BarcodeDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BarcodeDetailResponseDto>> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _barcodeService.GetDetailsAsync(
                id,
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/barcode/code/{code}
    // Get Barcode By Code
    // ================================================================

    [HttpGet("code/{code}")]
    [ProducesResponseType(
        typeof(BarcodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BarcodeResponseDto>> GetByCode(
        string code,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _barcodeService.GetByCodeAsync(
                code,
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/barcode/asset/{assetId}
    // Get Barcode By Asset
    // ================================================================

    [HttpGet("asset/{assetId:guid}")]
    [ProducesResponseType(
        typeof(BarcodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BarcodeResponseDto>> GetByAssetId(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _barcodeService.GetByAssetIdAsync(
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/barcode/asset/{assetId}/active
    // Get Active Barcode By Asset
    // ================================================================

    [HttpGet("asset/{assetId:guid}/active")]
    [ProducesResponseType(
        typeof(BarcodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BarcodeResponseDto>> GetActiveByAssetId(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _barcodeService.GetActiveByAssetIdAsync(
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/barcode
    // Get All Barcodes
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(BarcodeListResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BarcodeListResponseDto>> GetAll(
        [FromQuery] BarcodeFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _barcodeService.GetAllAsync(
                request,
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
    // POST: api/barcode/generate
    // Generate Barcode
    // ================================================================

    [HttpPost("generate")]
    [ProducesResponseType(
        typeof(BarcodeResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BarcodeResponseDto>> Generate(
        [FromBody] GenerateBarcodeRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _barcodeService.GenerateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = result.Id
                },
                result);
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
    // PUT: api/barcode/{id}
    // Update Barcode
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(BarcodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BarcodeResponseDto>> Update(
        Guid id,
        [FromBody] UpdateBarcodeRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _barcodeService.UpdateAsync(
                id,
                request,
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // DELETE: api/barcode/{id}
    // Delete Barcode
    // ================================================================

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _barcodeService.DeleteAsync(
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
    }
}

