using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Permission.Requests;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(
        IPermissionService permissionService)
    {
        _permissionService = permissionService
            ?? throw new ArgumentNullException(
                nameof(permissionService));
    }


    // ============================================================
    // Create
    // ============================================================

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePermissionRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _permissionService.CreateAsync(
                    request,
                    cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
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


    // ============================================================
    // Get By ID
    // ============================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _permissionService.GetByIdAsync(
                    id,
                    cancellationToken);

            if (result is null)
            {
                return NotFound(new
                {
                    message =
                        $"Permission with ID '{id}' was not found."
                });
            }

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


    // ============================================================
    // Get By Name
    // ============================================================

    [HttpGet("by-name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName(
        [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _permissionService.GetByNameAsync(
                    name,
                    cancellationToken);

            if (result is null)
            {
                return NotFound(new
                {
                    message =
                        $"Permission '{name}' was not found."
                });
            }

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


    // ============================================================
    // Get By Module
    // ============================================================

    [HttpGet("by-module")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByModule(
        [FromQuery] string module,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _permissionService.GetByModuleAsync(
                    module,
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


    // ============================================================
    // Get Active Permissions
    // ============================================================

    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(
        CancellationToken cancellationToken)
    {
        var result =
            await _permissionService.GetActiveAsync(
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Get All / Filter
    // ============================================================

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PermissionFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _permissionService.GetAllAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Update
    // ============================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePermissionRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _permissionService.UpdateAsync(
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
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ============================================================
    // Activate
    // ============================================================

    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(
        Guid id,
        [FromQuery] Guid updatedBy,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _permissionService.ActivateAsync(
                    id,
                    updatedBy,
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
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ============================================================
    // Deactivate
    // ============================================================

    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(
        Guid id,
        [FromQuery] Guid updatedBy,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _permissionService.DeactivateAsync(
                    id,
                    updatedBy,
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
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ============================================================
    // Soft Delete
    // ============================================================

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromQuery] Guid deletedBy,
        CancellationToken cancellationToken)
    {
        try
        {
            await _permissionService.DeleteAsync(
                id,
                deletedBy,
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

