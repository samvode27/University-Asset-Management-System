using Microsoft.EntityFrameworkCore;
using UAMS.Application.Interfaces.Repositories;
using UAMS.Domain.Entities.Suppliers;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure.Repositories;

public class SupplierRepository
    : GenericRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(UAMSDbContext context)
        : base(context)
    {
    }


    // ================================================================
    // Get Supplier By Name
    // ================================================================

    public virtual async Task<Supplier?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                supplier => supplier.Name == name,
                cancellationToken);
    }


    public async Task<Supplier?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(supplier => supplier.Purchases)
            .FirstOrDefaultAsync(
                supplier => supplier.Id == id,
                cancellationToken);
    }

    // ================================================================
    // Get Active Suppliers
    // ================================================================

    public virtual async Task<IReadOnlyList<Supplier>>
        GetActiveSuppliersAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(supplier => supplier.IsActive)
            .OrderBy(supplier => supplier.Name)
            .ToListAsync(cancellationToken);
    }


    // ================================================================
    // Get Inactive Suppliers
    // ================================================================

    public virtual async Task<IReadOnlyList<Supplier>>
        GetInactiveSuppliersAsync(
            CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(supplier => !supplier.IsActive)
            .OrderBy(supplier => supplier.Name)
            .ToListAsync(cancellationToken);
    }
}