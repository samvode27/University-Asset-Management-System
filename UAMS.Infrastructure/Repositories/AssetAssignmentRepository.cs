using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.AssetAssignments;
using UAMS.Domain.Enums;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class AssetAssignmentRepository
    : GenericRepository<AssetAssignment>, IAssetAssignmentRepository
{
    public AssetAssignmentRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Assignments By Asset
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetAssignment>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(assignment => assignment.AssetId == assetId)
            .OrderByDescending(assignment => assignment.AssignedDate)
            .ToListAsync(cancellationToken);
    }


    public virtual async Task<IReadOnlyList<AssetAssignment>>
    GetByStatusAsync(
        AssetAssignmentStatus status,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(assignment => assignment.Status == status)
            .OrderByDescending(assignment => assignment.AssignedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Assignments By Employee
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetAssignment>>
        GetByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(assignment => assignment.EmployeeId == employeeId)
            .OrderByDescending(assignment => assignment.AssignedDate)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Assignment By Asset Request
    // ================================================================

    public virtual async Task<AssetAssignment?>
        GetByAssetRequestIdAsync(
            Guid assetRequestId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                assignment =>
                    assignment.AssetRequestId == assetRequestId,
                cancellationToken);
    }


    // ================================================================
    // Get Active Assignment By Asset
    // ================================================================

    public virtual async Task<AssetAssignment?>
        GetActiveByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                assignment =>
                    assignment.AssetId == assetId &&
                    assignment.ActualReturnDate == null,
                cancellationToken);
    }


    // ================================================================
    // Get Active Assignments By Employee
    // ================================================================

    public virtual async Task<IReadOnlyList<AssetAssignment>>
        GetActiveByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(assignment =>
                assignment.EmployeeId == employeeId &&
                assignment.ActualReturnDate == null)
            .OrderByDescending(assignment => assignment.AssignedDate)
            .ToListAsync(cancellationToken);
    }
}