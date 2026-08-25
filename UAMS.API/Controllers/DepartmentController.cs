using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAMS.Application.DTOs.Departments.Requests;
using UAMS.Application.DTOs.Departments.Responses;
using UAMS.Application.Interfaces.Services;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(
        IDepartmentService departmentService)
    {
        _departmentService = departmentService
            ?? throw new ArgumentNullException(nameof(departmentService));
    }


    // ================================================================
    // GET: api/Department
    // Get departments with filtering and pagination
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(DepartmentListResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<DepartmentListResponseDto>> GetAll(
        [FromQuery] DepartmentFilterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/Department/{id}
    // Get department summary
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(DepartmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // GET: api/Department/{id}/details
    // Get complete department details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(DepartmentDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentDetailResponseDto>> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetDetailsAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // POST: api/Department
    // Create department
    // ================================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(DepartmentResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponseDto>> Create(
        [FromBody] CreateDepartmentRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // ================================================================
    // PUT: api/Department/{id}
    // Update department
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(DepartmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponseDto>> Update(
        Guid id,
        [FromBody] UpdateDepartmentRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // PATCH: api/Department/{id}/activate
    // Activate department
    // ================================================================

    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(
        typeof(DepartmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponseDto>> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.ActivateAsync(
            id,
            cancellationToken);

        return Ok(result);
    }


    // ================================================================
    // PATCH: api/Department/{id}/deactivate
    // Deactivate department
    // ================================================================

    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(
        typeof(DepartmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponseDto>> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.DeactivateAsync(
            id,
            cancellationToken);

        return Ok(result);
    }
}

