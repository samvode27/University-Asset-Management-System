using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.DamageReports.Requests;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DamageReportController : ControllerBase
{
    private readonly IDamageReportService _damageReportService;

    public DamageReportController(
        IDamageReportService damageReportService)
    {
        _damageReportService = damageReportService;
    }


    // ============================================================
    // GET: api/DamageReport
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DamageReportFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _damageReportService.GetDamageReportsAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // GET: api/DamageReport/{id}
    // ============================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result =
            await _damageReportService.GetDamageReportAsync(
                id,
                cancellationToken);

        if (result is null)
        {
            return NotFound(
                new
                {
                    message = "Damage report not found."
                });
        }

        return Ok(result);
    }


    // ============================================================
    // GET: api/DamageReport/{id}/details
    // ============================================================

    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result =
            await _damageReportService
                .GetDamageReportDetailsAsync(
                    id,
                    cancellationToken);

        if (result is null)
        {
            return NotFound(
                new
                {
                    message = "Damage report not found."
                });
        }

        return Ok(result);
    }


    // ============================================================
    // POST: api/DamageReport
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDamageReportRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (!userId.HasValue)
        {
            return Unauthorized(
                new
                {
                    message = "Authenticated user ID could not be determined."
                });
        }

        var result =
            await _damageReportService
                .CreateDamageReportAsync(
                    userId.Value,
                    request,
                    cancellationToken);

        if (result is null)
        {
            return BadRequest(
                new
                {
                    message =
                        "The specified asset, assignment, or user could not be found."
                });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // ============================================================
    // PUT: api/DamageReport/{id}
    // ============================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDamageReportRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _damageReportService
                    .UpdateDamageReportAsync(
                        id,
                        request,
                        cancellationToken);

            if (result is null)
            {
                return NotFound(
                    new
                    {
                        message = "Damage report not found."
                    });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ============================================================
    // POST: api/DamageReport/{id}/review
    // ============================================================

    [HttpPost("{id:guid}/review")]
    public async Task<IActionResult> StartReview(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _damageReportService
                    .StartReviewAsync(
                        id,
                        cancellationToken);

            if (result is null)
            {
                return NotFound(
                    new
                    {
                        message = "Damage report not found."
                    });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ============================================================
    // POST: api/DamageReport/{id}/assess
    // ============================================================

    [HttpPost("{id:guid}/assess")]
    public async Task<IActionResult> Assess(
        Guid id,
        [FromBody] AssessDamageReportRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (!userId.HasValue)
        {
            return Unauthorized(
                new
                {
                    message =
                        "Authenticated user ID could not be determined."
                });
        }

        try
        {
            var result =
                await _damageReportService
                    .AssessDamageReportAsync(
                        id,
                        userId.Value,
                        request,
                        cancellationToken);

            if (result is null)
            {
                return NotFound(
                    new
                    {
                        message = "Damage report or assessor not found."
                    });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ============================================================
    // POST: api/DamageReport/{id}/resolve
    // ============================================================

    [HttpPost("{id:guid}/resolve")]
    public async Task<IActionResult> Resolve(
        Guid id,
        [FromBody] ResolveDamageReportRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _damageReportService
                    .ResolveDamageReportAsync(
                        id,
                        request,
                        cancellationToken);

            if (result is null)
            {
                return NotFound(
                    new
                    {
                        message = "Damage report not found."
                    });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ============================================================
    // POST: api/DamageReport/{id}/reject
    // ============================================================

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid id,
        [FromBody] RejectDamageReportRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (!userId.HasValue)
        {
            return Unauthorized(
                new
                {
                    message =
                        "Authenticated user ID could not be determined."
                });
        }

        try
        {
            var result =
                await _damageReportService
                    .RejectDamageReportAsync(
                        id,
                        userId.Value,
                        request,
                        cancellationToken);

            if (result is null)
            {
                return NotFound(
                    new
                    {
                        message = "Damage report or assessor not found."
                    });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ============================================================
    // POST: api/DamageReport/{id}/cancel
    // ============================================================

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _damageReportService
                    .CancelDamageReportAsync(
                        id,
                        cancellationToken);

            if (result is null)
            {
                return NotFound(
                    new
                    {
                        message = "Damage report not found."
                    });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new
                {
                    message = ex.Message
                });
        }
    }


    // ============================================================
    // Get Current User ID
    // ============================================================

    private Guid? GetCurrentUserId()
    {
        var value =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        return Guid.TryParse(
            value,
            out var userId)
            ? userId
            : null;
    }
}