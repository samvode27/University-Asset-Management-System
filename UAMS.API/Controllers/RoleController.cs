using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UAMS.Application.DTOs.Roles.Requests;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }


    // ================================================================
    // Create Role
    // POST: api/Role
    // ================================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.CreateAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get Role By ID
    // GET: api/Role/{id}
    // ================================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get Role Details
    // GET: api/Role/{id}/details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.GetDetailsAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get Roles
    // GET: api/Role
    // ================================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _roleService.GetAllAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Update Role
    // PUT: api/Role/{id}
    // ================================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateRoleRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Assign Permissions
    // POST: api/Role/{id}/permissions
    // ================================================================

    [HttpPost("{id:guid}/permissions")]
    public async Task<IActionResult> AssignPermissions(
        Guid id,
        [FromBody] AssignPermissionsRequestDto request,
        CancellationToken cancellationToken)
    {
       await _roleService.AssignPermissionsAsync(
           id,
           request,
           cancellationToken);

       return NoContent();
    }


    // ================================================================
    // Remove Permissions
    // DELETE: api/Role/{id}/permissions
    // ================================================================

    [HttpDelete("{id:guid}/permissions")]
    public async Task<IActionResult> RemovePermissions(
        Guid id,
        [FromBody] RemovePermissionsRequestDto request,
        CancellationToken cancellationToken)
    {
       await _roleService.RemovePermissionsAsync(
          id,
          request,
          cancellationToken);

       return NoContent();
    }


    // ================================================================
    // Get Active Roles
    // GET: api/Role/active
    // ================================================================

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveRoles(
        CancellationToken cancellationToken)
    {
        var result = await _roleService.GetActiveRolesAsync(
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // Get System Roles
    // GET: api/Role/system
    // ================================================================

    [HttpGet("system")]
    public async Task<IActionResult> GetSystemRoles(
        CancellationToken cancellationToken)
    {
        var result = await _roleService.GetSystemRolesAsync(
            cancellationToken);

        return Ok(result);
    }
}