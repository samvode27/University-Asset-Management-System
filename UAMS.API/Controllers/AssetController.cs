using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Assets.Requests;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetController(IAssetService assetService)
    {
        _assetService = assetService
            ?? throw new ArgumentNullException(nameof(assetService));
    }


    // ================================================================
    // GET: api/asset
    // Get Assets
    // ================================================================

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] AssetFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/asset/{id}
    // Get Asset By ID
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/asset/{id}/details
    // Get Asset Details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetService.GetDetailsAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // POST: api/asset
    // Create Asset
    // ================================================================

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssetRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // ================================================================
    // PUT: api/asset/{id}
    // Update Asset
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAssetRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // DELETE: api/asset/{id}
    // Delete Asset
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
        await _assetService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}