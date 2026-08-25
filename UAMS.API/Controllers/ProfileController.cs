using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Profile.Requests;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(
        IProfileService profileService)
    {
        _profileService = profileService;
    }


    // ============================================================
    // GET: api/profile
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetProfile(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var result =
            await _profileService.GetProfileAsync(
                userId.Value,
                cancellationToken);

        if (result is null)
        {
            return NotFound(
                new
                {
                    message = "Profile not found."
                });
        }

        return Ok(result);
    }


    // ============================================================
    // GET: api/profile/summary
    // ============================================================

    [HttpGet("summary")]
    public async Task<IActionResult> GetProfileSummary(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var result =
            await _profileService.GetProfileSummaryAsync(
                userId.Value,
                cancellationToken);

        if (result is null)
        {
            return NotFound(
                new
                {
                    message = "Profile not found."
                });
        }

        return Ok(result);
    }


    // ============================================================
    // PUT: api/profile
    // ============================================================

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var result =
            await _profileService.UpdateProfileAsync(
                userId.Value,
                request,
                cancellationToken);

        if (result is null)
        {
            return NotFound(
                new
                {
                    message = "Profile not found."
                });
        }

        return Ok(result);
    }


    // ============================================================
    // PUT: api/profile/picture
    // ============================================================

    [HttpPut("picture")]
    public async Task<IActionResult> UpdateProfilePicture(
        [FromBody] UpdateProfilePictureRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var result =
            await _profileService.UpdateProfilePictureAsync(
                userId.Value,
                request,
                cancellationToken);

        if (result is null)
        {
            return NotFound(
                new
                {
                    message = "User profile not found."
                });
        }

        return Ok(result);
    }


    // ============================================================
    // PUT: api/profile/preferences
    // ============================================================

    [HttpPut("preferences")]
    public async Task<IActionResult> UpdatePreferences(
        [FromBody] UpdateProfilePreferencesRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            var result =
                await _profileService.UpdatePreferencesAsync(
                    userId.Value,
                    request,
                    cancellationToken);

            if (result is null)
            {
                return NotFound(
                    new
                    {
                        message = "Profile not found."
                    });
            }

            return Ok(result);
        }
        catch (NotSupportedException ex)
        {
            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = ex.Message
                });
        }
    }


    // ============================================================
    // Current User ID
    // ============================================================

    private Guid? GetCurrentUserId()
    {
        var claim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

        if (claim is null)
        {
            return null;
        }

        return Guid.TryParse(
            claim.Value,
            out var userId)
            ? userId
            : null;
    }
}