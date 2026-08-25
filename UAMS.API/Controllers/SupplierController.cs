using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Suppliers.Requests;
using UAMS.Application.DTOs.Suppliers.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(
        ISupplierService supplierService)
    {
        _supplierService = supplierService
            ?? throw new ArgumentNullException(nameof(supplierService));
    }


    // ================================================================
    // GET: api/Supplier/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(SupplierResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupplierResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _supplierService.GetByIdAsync(
                id,
                cancellationToken);

            return Ok(supplier);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message = $"Supplier with ID '{id}' was not found."
            });
        }
    }


    // ================================================================
    // GET: api/Supplier/{id}/details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(SupplierDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupplierDetailResponseDto>> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _supplierService.GetDetailsAsync(
                id,
                cancellationToken);

            return Ok(supplier);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message = $"Supplier with ID '{id}' was not found."
            });
        }
    }


    // ================================================================
    // GET: api/Supplier
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(SupplierListResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<SupplierListResponseDto>> GetAll(
        [FromQuery] SupplierFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _supplierService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // POST: api/Supplier
    // ================================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(SupplierResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SupplierResponseDto>> Create(
        [FromBody] CreateSupplierRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _supplierService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = supplier.Id },
                supplier);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                message = exception.Message
            });
        }
    }


    // ================================================================
    // PUT: api/Supplier/{id}
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(SupplierResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SupplierResponseDto>> Update(
        Guid id,
        [FromBody] UpdateSupplierRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await _supplierService.UpdateAsync(
                id,
                request,
                cancellationToken);

            return Ok(supplier);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message = $"Supplier with ID '{id}' was not found."
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                message = exception.Message
            });
        }
    }


    // ================================================================
    // PATCH: api/Supplier/{id}/activate
    // ================================================================

    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _supplierService.ActivateAsync(
                id,
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message = $"Supplier with ID '{id}' was not found."
            });
        }
    }


    // ================================================================
    // PATCH: api/Supplier/{id}/deactivate
    // ================================================================

    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _supplierService.DeactivateAsync(
                id,
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message = $"Supplier with ID '{id}' was not found."
            });
        }
    }


    // ================================================================
    // DELETE: api/Supplier/{id}
    // ================================================================

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _supplierService.DeleteAsync(
                id,
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message = $"Supplier with ID '{id}' was not found."
            });
        }
    }
}

