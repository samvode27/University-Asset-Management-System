using UAMS.Application.DTOs.Permission.Requests;
using UAMS.Application.DTOs.Permission.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IPermissionService
{
    // ============================================================
    // Create
    // ============================================================

    Task<PermissionResponseDto> CreateAsync(
        CreatePermissionRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By ID
    // ============================================================

    Task<PermissionDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By Name
    // ============================================================

    Task<PermissionResponseDto?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By Module
    // ============================================================

    Task<IReadOnlyList<PermissionResponseDto>> GetByModuleAsync(
        string module,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get Active Permissions
    // ============================================================

    Task<IReadOnlyList<PermissionResponseDto>> GetActiveAsync(
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get All / Filter
    // ============================================================

    Task<PermissionListResponseDto> GetAllAsync(
        PermissionFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Update
    // ============================================================

    Task<PermissionResponseDto> UpdateAsync(
        Guid id,
        UpdatePermissionRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Activate
    // ============================================================

    Task<PermissionResponseDto> ActivateAsync(
        Guid id,
        Guid updatedBy,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Deactivate
    // ============================================================

    Task<PermissionResponseDto> DeactivateAsync(
        Guid id,
        Guid updatedBy,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Soft Delete
    // ============================================================

    Task DeleteAsync(
        Guid id,
        Guid deletedBy,
        CancellationToken cancellationToken = default);
}

