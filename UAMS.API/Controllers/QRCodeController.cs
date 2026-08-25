using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.QRCode.Requests;
using UAMS.Application.DTOs.QRCode.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/qr-codes")]
[Authorize]
public class QRCodeController : ControllerBase
{
    private readonly IQRCodeService _qrCodeService;

    public QRCodeController(IQRCodeService qrCodeService)
    {
        _qrCodeService = qrCodeService
            ?? throw new ArgumentNullException(nameof(qrCodeService));
    }


    // ================================================================
    // GET: api/qr-codes
    // ================================================================

    /// <summary>
    /// Gets a paginated and filtered list of QR codes.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(QRCodeListResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<QRCodeListResponseDto>> GetAll(
        [FromQuery] QRCodeFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _qrCodeService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/qr-codes/{id}
    // ================================================================

    /// <summary>
    /// Gets a QR code by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(QRCodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QRCodeResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _qrCodeService.GetByIdAsync(
                id,
                cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


    // ================================================================
    // GET: api/qr-codes/{id}/details
    // ================================================================

    /// <summary>
    /// Gets detailed information about a QR code and its asset.
    /// </summary>
    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(QRCodeDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QRCodeDetailResponseDto>> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _qrCodeService.GetDetailsAsync(
                id,
                cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


    // ================================================================
    // GET: api/qr-codes/code/{code}
    // ================================================================

    /// <summary>
    /// Gets a QR code by its code value.
    /// </summary>
    [HttpGet("code/{code}")]
    [ProducesResponseType(
        typeof(QRCodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QRCodeResponseDto>> GetByCode(
        string code,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _qrCodeService.GetByCodeAsync(
                code,
                cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest(
                new
                {
                    message = "QR code is required."
                });
        }
    }


    // ================================================================
    // GET: api/qr-codes/asset/{assetId}
    // ================================================================

    /// <summary>
    /// Gets the QR code associated with an asset.
    /// </summary>
    [HttpGet("asset/{assetId:guid}")]
    [ProducesResponseType(
        typeof(QRCodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QRCodeResponseDto>> GetByAssetId(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _qrCodeService.GetByAssetIdAsync(
                assetId,
                cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest(
                new
                {
                    message = "Asset ID is required."
                });
        }
    }


    // ================================================================
    // GET: api/qr-codes/asset/{assetId}/active
    // ================================================================

    /// <summary>
    /// Gets the active QR code associated with an asset.
    /// </summary>
    [HttpGet("asset/{assetId:guid}/active")]
    [ProducesResponseType(
        typeof(QRCodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QRCodeResponseDto>>
        GetActiveByAssetId(
            Guid assetId,
            CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _qrCodeService.GetActiveByAssetIdAsync(
                    assetId,
                    cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest(
                new
                {
                    message = "Asset ID is required."
                });
        }
    }


    // ================================================================
    // POST: api/qr-codes
    // ================================================================

    /// <summary>
    /// Generates a new QR code for an asset.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(QRCodeResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<QRCodeResponseDto>> Generate(
        [FromBody] GenerateQRCodeRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _qrCodeService.GenerateAsync(
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
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                new
                {
                    message = ex.Message
                });
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
    // PUT: api/qr-codes/{id}
    // ================================================================

    /// <summary>
    /// Updates the expiration date of a QR code.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(QRCodeResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QRCodeResponseDto>> Update(
        Guid id,
        [FromBody] UpdateQRCodeRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _qrCodeService.UpdateAsync(
                id,
                request,
                cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
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
    // DELETE: api/qr-codes/{id}
    // ================================================================

    /// <summary>
    /// Deletes a QR code.
    /// </summary>
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
            await _qrCodeService.DeleteAsync(
                id,
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
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
}
