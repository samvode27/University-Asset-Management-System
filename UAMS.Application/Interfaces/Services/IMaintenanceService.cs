using UAMS.Application.DTOs.Maintenance.Requests;
using UAMS.Application.DTOs.Maintenance.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IMaintenanceService
{
    // ============================================================
    // Create
    // ============================================================

    Task<MaintenanceResponseDto> CreateAsync(
        CreateMaintenanceRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By ID
    // ============================================================

    Task<MaintenanceDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By Maintenance Number
    // ============================================================

    Task<MaintenanceDetailResponseDto?> GetByMaintenanceNumberAsync(
        string maintenanceNumber,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get All / Filter
    // ============================================================

    Task<MaintenanceListResponseDto> GetAllAsync(
        MaintenanceFilterRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By Asset
    // ============================================================

    Task<IReadOnlyList<MaintenanceResponseDto>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By Damage Report
    // ============================================================

    Task<IReadOnlyList<MaintenanceResponseDto>> GetByDamageReportIdAsync(
        Guid damageReportId,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By Requested User
    // ============================================================

    Task<IReadOnlyList<MaintenanceResponseDto>> GetByRequestedByIdAsync(
        Guid requestedById,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get By Assigned Technician
    // ============================================================

    Task<IReadOnlyList<MaintenanceResponseDto>>
        GetByAssignedTechnicianIdAsync(
            Guid technicianId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // Get Pending Maintenance
    // ============================================================

    Task<IReadOnlyList<MaintenanceResponseDto>> GetPendingAsync(
        CancellationToken cancellationToken = default);


    // ============================================================
    // Get Open Maintenance
    // ============================================================

    Task<IReadOnlyList<MaintenanceResponseDto>> GetOpenAsync(
        CancellationToken cancellationToken = default);


    // ============================================================
    // Update
    // ============================================================

    Task<MaintenanceResponseDto> UpdateAsync(
        Guid id,
        UpdateMaintenanceRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Assign Technician
    // ============================================================

    Task<MaintenanceResponseDto> AssignTechnicianAsync(
        Guid id,
        AssignMaintenanceTechnicianRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Start Maintenance
    // ============================================================

    Task<MaintenanceResponseDto> StartAsync(
        Guid id,
        StartMaintenanceRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Complete Maintenance
    // ============================================================

    Task<MaintenanceResponseDto> CompleteAsync(
        Guid id,
        CompleteMaintenanceRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Cancel Maintenance
    // ============================================================

    Task<MaintenanceResponseDto> CancelAsync(
        Guid id,
        CancelMaintenanceRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Activate
    // ============================================================

    Task<MaintenanceResponseDto> ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);


    // ============================================================
    // Soft Delete
    // ============================================================

    Task DeleteAsync(
        Guid id,
        Guid deletedBy,
        CancellationToken cancellationToken = default);
}

