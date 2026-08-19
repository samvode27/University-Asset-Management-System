using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Maintenances;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class MaintenanceRequestRepository
    : GenericRepository<Maintenance>, IMaintenanceRequestRepository
{
    public MaintenanceRequestRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Maintenance By Maintenance Number
    // ================================================================

    public virtual async Task<Maintenance?>
        GetByMaintenanceNumberAsync(
            string maintenanceNumber,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                maintenance =>
                    maintenance.MaintenanceNumber == maintenanceNumber,
                cancellationToken);
    }


    // ================================================================
    // Get Maintenance Records By Asset
    // ================================================================

    public virtual async Task<IReadOnlyList<Maintenance>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(maintenance =>
                maintenance.AssetId == assetId)
            .OrderByDescending(maintenance =>
                maintenance.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Maintenance Records By Damage Report
    // ================================================================

    public virtual async Task<IReadOnlyList<Maintenance>>
        GetByDamageReportIdAsync(
            Guid damageReportId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(maintenance =>
                maintenance.DamageReportId == damageReportId)
            .OrderByDescending(maintenance =>
                maintenance.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Maintenance Records Requested By User
    // ================================================================

    public virtual async Task<IReadOnlyList<Maintenance>>
        GetByRequestedByIdAsync(
            Guid requestedById,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(maintenance =>
                maintenance.RequestedById == requestedById)
            .OrderByDescending(maintenance =>
                maintenance.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Maintenance Records Assigned To Technician
    // ================================================================

    public virtual async Task<IReadOnlyList<Maintenance>>
        GetByAssignedTechnicianIdAsync(
            Guid technicianId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(maintenance =>
                maintenance.AssignedTechnicianId == technicianId)
            .OrderByDescending(maintenance =>
                maintenance.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Maintenance By Maintenance Type
    // ================================================================

    public virtual async Task<IReadOnlyList<Maintenance>>
        GetByMaintenanceTypeAsync(
            MaintenanceType maintenanceType,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(maintenance =>
                maintenance.MaintenanceType == maintenanceType)
            .OrderByDescending(maintenance =>
                maintenance.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Maintenance By Status
    // ================================================================

    public virtual async Task<IReadOnlyList<Maintenance>>
        GetByStatusAsync(
            MaintenanceStatus status,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(maintenance =>
                maintenance.Status == status)
            .OrderByDescending(maintenance =>
                maintenance.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Pending Maintenance
    // ================================================================

    public virtual async Task<IReadOnlyList<Maintenance>>
        GetPendingAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(maintenance =>
                maintenance.Status == MaintenanceStatus.Pending)
            .OrderBy(maintenance =>
                maintenance.RequestedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Open Maintenance
    // ================================================================

    public virtual async Task<IReadOnlyList<Maintenance>>
        GetOpenAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(maintenance =>
                maintenance.Status == MaintenanceStatus.Pending ||
                maintenance.Status == MaintenanceStatus.Approved ||
                maintenance.Status == MaintenanceStatus.InProgress)
            .OrderBy(maintenance =>
                maintenance.RequestedDate)
            .ToListAsync(cancellationToken);
    }
}