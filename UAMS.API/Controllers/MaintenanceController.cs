using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Maintenance.Requests;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenanceService _maintenanceService;

    public MaintenanceController(
        IMaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }


    // ============================================================
    // Create
    // POST: api/Maintenance
    // ============================================================

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateMaintenanceRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _maintenanceService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
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
    // Get By ID
    // GET: api/Maintenance/{id}
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
            var result = await _maintenanceService.GetByIdAsync(
                id,
                cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }


    // ============================================================
    // Get By Maintenance Number
    // GET: api/Maintenance/number/{maintenanceNumber}
    // ============================================================

    [HttpGet("number/{maintenanceNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByMaintenanceNumber(
        string maintenanceNumber,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _maintenanceService.GetByMaintenanceNumberAsync(
                    maintenanceNumber,
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


    // ============================================================
    // Get All / Filter
    // GET: api/Maintenance
    // ============================================================

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] MaintenanceFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _maintenanceService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Get By Asset
    // GET: api/Maintenance/asset/{assetId}
    // ============================================================

    [HttpGet("asset/{assetId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAssetId(
        Guid assetId,
        CancellationToken cancellationToken)
    {
        var result =
            await _maintenanceService.GetByAssetIdAsync(
                assetId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Get By Damage Report
    // GET: api/Maintenance/damage-report/{damageReportId}
    // ============================================================

    [HttpGet("damage-report/{damageReportId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDamageReportId(
        Guid damageReportId,
        CancellationToken cancellationToken)
    {
        var result =
            await _maintenanceService.GetByDamageReportIdAsync(
                damageReportId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Get By Requested User
    // GET: api/Maintenance/requested-by/{requestedById}
    // ============================================================

    [HttpGet("requested-by/{requestedById:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRequestedById(
        Guid requestedById,
        CancellationToken cancellationToken)
    {
        var result =
            await _maintenanceService.GetByRequestedByIdAsync(
                requestedById,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Get By Assigned Technician
    // GET: api/Maintenance/technician/{technicianId}
    // ============================================================

    [HttpGet("technician/{technicianId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAssignedTechnicianId(
        Guid technicianId,
        CancellationToken cancellationToken)
    {
        var result =
            await _maintenanceService.GetByAssignedTechnicianIdAsync(
                technicianId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Get Pending Maintenance
    // GET: api/Maintenance/pending
    // ============================================================

    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(
        CancellationToken cancellationToken)
    {
        var result =
            await _maintenanceService.GetPendingAsync(
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Get Open Maintenance
    // GET: api/Maintenance/open
    // ============================================================

    [HttpGet("open")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpen(
        CancellationToken cancellationToken)
    {
        var result =
            await _maintenanceService.GetOpenAsync(
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // Update
    // PUT: api/Maintenance/{id}
    // ============================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateMaintenanceRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _maintenanceService.UpdateAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
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
    // Assign Technician
    // PATCH: api/Maintenance/{id}/assign-technician
    // ============================================================

    [HttpPatch("{id:guid}/assign-technician")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignTechnician(
        Guid id,
        [FromBody] AssignMaintenanceTechnicianRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _maintenanceService.AssignTechnicianAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
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
    // Start Maintenance
    // PATCH: api/Maintenance/{id}/start
    // ============================================================

    [HttpPatch("{id:guid}/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Start(
        Guid id,
        [FromBody] StartMaintenanceRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _maintenanceService.StartAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
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
    // Complete Maintenance
    // PATCH: api/Maintenance/{id}/complete
    // ============================================================

    [HttpPatch("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromBody] CompleteMaintenanceRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _maintenanceService.CompleteAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
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
    // Cancel Maintenance
    // PATCH: api/Maintenance/{id}/cancel
    // ============================================================

    [HttpPatch("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromBody] CancelMaintenanceRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _maintenanceService.CancelAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(result);
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
    // PATCH: api/Maintenance/{id}/activate
    // ============================================================

    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _maintenanceService.ActivateAsync(
                    id,
                    cancellationToken);

            return Ok(result);
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
    // DELETE: api/Maintenance/{id}
    // ============================================================

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromQuery] Guid deletedBy,
        CancellationToken cancellationToken)
    {
        try
        {
            if (deletedBy == Guid.Empty)
            {
                return BadRequest(new
                {
                    message = "DeletedBy user ID is required."
                });
            }

            await _maintenanceService.DeleteAsync(
                id,
                deletedBy,
                cancellationToken);

            return NoContent();
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