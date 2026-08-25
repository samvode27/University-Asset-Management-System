using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.AssetCategories.Requests;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetCategoryController : ControllerBase
{
    private readonly IAssetCategoryService _assetCategoryService;

    public AssetCategoryController(
        IAssetCategoryService assetCategoryService)
    {
        _assetCategoryService = assetCategoryService
            ?? throw new ArgumentNullException(
                nameof(assetCategoryService));
    }


    // ================================================================
    // GET: api/AssetCategory/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetCategoryService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AssetCategory/{id}/details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetCategoryService.GetDetailsAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/AssetCategory
    // ================================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] AssetCategoryFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetCategoryService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // POST: api/AssetCategory
    // ================================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssetCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetCategoryService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // ================================================================
    // PUT: api/AssetCategory/{id}
    // ================================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAssetCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _assetCategoryService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // PATCH: api/AssetCategory/{id}/activate
    // ================================================================

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetCategoryService.ActivateAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // PATCH: api/AssetCategory/{id}/deactivate
    // ================================================================

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _assetCategoryService.DeactivateAsync(
            id,
            cancellationToken);

        return Ok(result);
    }
}

