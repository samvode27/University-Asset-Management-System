using Microsoft.EntityFrameworkCore;
using UAMS.Domain.Entities.AssetAssignments;
using UAMS.Domain.Entities.AssetCategories;
using UAMS.Domain.Entities.AssetDisposals;
using UAMS.Domain.Entities.AssetRequests;
using UAMS.Domain.Entities.AssetReturns;
using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.AssetTransfers;
using UAMS.Domain.Entities.AuditLogs;
using UAMS.Domain.Entities.Barcodes;
using UAMS.Domain.Entities.DamageReports;
using UAMS.Domain.Entities.Departments;
using UAMS.Domain.Entities.FileAttachments;
using UAMS.Domain.Entities.Maintenances;
using UAMS.Domain.Entities.Notifications;
using UAMS.Domain.Entities.Permissions;
using UAMS.Domain.Entities.Purchases;
using UAMS.Domain.Entities.QRCodes;
using UAMS.Domain.Entities.Roles;
using UAMS.Domain.Entities.Suppliers;
using UAMS.Domain.Entities.Users;

namespace UAMS.Infrastructure.Persistence;

public class UAMSDbContext : DbContext
{
    public UAMSDbContext(DbContextOptions<UAMSDbContext> options)
        : base(options)
    {
    }


    // ================================================================
    // Security & Access Control
    // ================================================================

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // ================================================================
    // Organization
    // ================================================================

    public DbSet<Department> Departments { get; set; }


    // ================================================================
    // Procurement
    // ================================================================

    public DbSet<Purchase> Purchases { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }


    // ================================================================
    // Assets
    // ================================================================

    public DbSet<Asset> Assets { get; set; }

    public DbSet<QRCode> QRCodes { get; set; }

    public DbSet<Barcode> Barcodes { get; set; }

    public DbSet<AssetCategory> AssetCategories { get; set; }

    // ================================================================
    // Asset Request & Assignment
    // ================================================================

    public DbSet<AssetRequest> AssetRequests { get; set; }

    public DbSet<AssetAssignment> AssetAssignments { get; set; }


    // ================================================================
    // Asset Transfer & Return
    // ================================================================

    public DbSet<AssetTransfer> AssetTransfers { get; set; }

    public DbSet<AssetReturn> AssetReturns { get; set; }


    // ================================================================
    // Damage & Maintenance
    // ================================================================

    public DbSet<DamageReport> DamageReports { get; set; }

    public DbSet<Maintenance> Maintenances { get; set; }


    // ================================================================
    // Asset Disposal
    // ================================================================

    public DbSet<AssetDisposal> AssetDisposals { get; set; }


    // ================================================================
    // Notifications & Files
    // ================================================================

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<FileAttachment> FileAttachments { get; set; }


    // ================================================================
    // Auditing
    // ================================================================

    public DbSet<AuditLog> AuditLogs { get; set; }


    // ================================================================
    // Model Configuration
    // ================================================================

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Automatically discovers and applies all
        // IEntityTypeConfiguration<T> implementations
        // located in the UAMS.Infrastructure assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UAMSDbContext).Assembly);
    }
}

