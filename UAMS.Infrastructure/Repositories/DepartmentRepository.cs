using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Departments;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class DepartmentRepository
    : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Department By Name
    // ================================================================

    public virtual async Task<Department?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                department => department.Name == name,
                cancellationToken);
    }

    // ================================================================
    // Get Department By Id With Details
    // ================================================================

    public virtual async Task<Department?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(id));
        }

        return await DbSet
            .Include(department => department.DepartmentHead)
            .Include(department => department.Users)
            .Include(department => department.Assets)
            .Include(department => department.AssetRequests)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                department => department.Id == id,
                cancellationToken);
    }


    // ================================================================
    // Get Active Departments
    // ================================================================

    public virtual async Task<IReadOnlyList<Department>>
        GetActiveDepartmentsAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(department => department.IsActive)
            .OrderBy(department => department.Name)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Inactive Departments
    // ================================================================

    public virtual async Task<IReadOnlyList<Department>>
        GetInactiveDepartmentsAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(department => !department.IsActive)
            .OrderBy(department => department.Name)
            .ToListAsync(cancellationToken);
    }
}