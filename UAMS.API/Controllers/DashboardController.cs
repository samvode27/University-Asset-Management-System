using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Dashboard.Requests;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(
        IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }


    // ============================================================
    // Get Dashboard
    // ============================================================

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDashboard(
        [FromQuery] DashboardFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _dashboardService.GetDashboardAsync(
                request,
                cancellationToken);

        return Ok(result);
    }
}