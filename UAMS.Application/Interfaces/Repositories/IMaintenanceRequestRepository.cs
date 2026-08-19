using UAMS.Domain.Entities.Maintenances;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IMaintenanceRequestRepository
    : IRepository<Maintenance>
{
    // ================================================================
    // Get Maintenance By Maintenance Number
    // ================================================================

    Task<Maintenance?> GetByMaintenanceNumberAsync(
        string maintenanceNumber,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Maintenance Records By Asset
    // ================================================================

    Task<IReadOnlyList<Maintenance>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Maintenance Records By Damage Report
    // ================================================================

    Task<IReadOnlyList<Maintenance>> GetByDamageReportIdAsync(
        Guid damageReportId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Maintenance Records Requested By User
    // ================================================================

    Task<IReadOnlyList<Maintenance>> GetByRequestedByIdAsync(
        Guid requestedById,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Maintenance Records Assigned To Technician
    // ================================================================

    Task<IReadOnlyList<Maintenance>> GetByAssignedTechnicianIdAsync(
        Guid technicianId,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Maintenance By Maintenance Type
    // ================================================================

    Task<IReadOnlyList<Maintenance>> GetByMaintenanceTypeAsync(
        MaintenanceType maintenanceType,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Maintenance By Status
    // ================================================================

    Task<IReadOnlyList<Maintenance>> GetByStatusAsync(
        MaintenanceStatus status,
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Pending Maintenance
    // ================================================================

    Task<IReadOnlyList<Maintenance>> GetPendingAsync(
        CancellationToken cancellationToken = default);


    // ================================================================
    // Get Open Maintenance
    // ================================================================

    Task<IReadOnlyList<Maintenance>> GetOpenAsync(
        CancellationToken cancellationToken = default);
}