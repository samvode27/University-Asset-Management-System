using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UAMS.Application.DTOs.Users.Requests;
using UAMS.Application.DTOs.Users.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(
        IUserService userService)
    {
        _userService = userService;
    }


    // ================================================================
    // Create User
    // POST: api/users
    // ================================================================

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create(
        [FromBody] CreateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _userService.CreateUserAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get User By ID
    // GET: api/users/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDetailResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result =
            await _userService.GetUserByIdAsync(
                id,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get Users
    // GET: api/users
    // ================================================================

    [HttpGet]
    public async Task<ActionResult<UserListResponseDto>> GetAll(
        [FromQuery] UserFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _userService.GetUsersAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Update User
    // PUT: api/users/{id}
    // ================================================================

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponseDto>> Update(
        Guid id,
        [FromBody] UpdateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _userService.UpdateUserAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Reset User Password
    // POST: api/users/{id}/reset-password
    // ================================================================

    [HttpPost("{id:guid}/reset-password")]
    public async Task<IActionResult> ResetPassword(
        Guid id,
        [FromBody] ResetUserPasswordRequestDto request,
        CancellationToken cancellationToken)
    {
        await _userService.ResetUserPasswordAsync(
            id,
            request,
            cancellationToken);

        return NoContent();
    }


    // ================================================================
    // Assign Role
    // POST: api/users/{id}/roles
    // ================================================================

    [HttpPost("{id:guid}/roles")]
    public async Task<IActionResult> AssignRole(
        Guid id,
        [FromBody] AssignRoleRequestDto request,
        CancellationToken cancellationToken)
    {
        await _userService.AssignRoleAsync(
            id,
            request,
            cancellationToken);

        return NoContent();
    }


    // ================================================================
    // Change Department
    // POST: api/users/{id}/department
    // ================================================================

    [HttpPost("{id:guid}/department")]
    public async Task<IActionResult> ChangeDepartment(
        Guid id,
        [FromBody] ChangeDepartmentRequestDto request,
        CancellationToken cancellationToken)
    {
        await _userService.ChangeDepartmentAsync(
            id,
            request,
            cancellationToken);

        return NoContent();
    }


    // ================================================================
    // Activate User
    // POST: api/users/{id}/activate
    // ================================================================

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _userService.ActivateUserAsync(
            id,
            cancellationToken);

        return NoContent();
    }


    // ================================================================
    // Deactivate User
    // POST: api/users/{id}/deactivate
    // ================================================================

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _userService.DeactivateUserAsync(
            id,
            cancellationToken);

        return NoContent();
    }


    // ================================================================
    // Delete User
    // DELETE: api/users/{id}
    // ================================================================

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _userService.DeleteUserAsync(
            id,
            cancellationToken);

        return NoContent();
    }


    // ================================================================
    // Restore Deleted User
    // ================================================================

    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _userService.RestoreAsync(
            id,
            cancellationToken);

        return Ok(new
        {
            succeeded = true,
            message = "User restored successfully."
        });
    }

    // ================================================================
    // Unlock User
    // ================================================================

    [HttpPost("{id:guid}/unlock")]
    public async Task<IActionResult> Unlock(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _userService.UnlockAsync(
            id,
            cancellationToken);

        return Ok(new
        {
            succeeded = true,
            message = "User unlocked successfully."
        });
    }
}