using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UAMS.Domain.Entities.Assets;
using UAMS.Domain.Entities.Barcodes;
using UAMS.Domain.Entities.QRCodes;
using UAMS.Domain.Entities.AssetDisposals;

namespace UAMS.Infrastructure.Configurations;

public class AssetConfiguration
    : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Assets", "AssetManagement");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Asset Tag
        // ============================================================

        builder.Property(a => a.AssetTag)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Asset Name
        // ============================================================

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(true);


        // ============================================================
        // Description
        // ============================================================

        builder.Property(a => a.Description)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Serial Number
        // ============================================================

        builder.Property(a => a.SerialNumber)
            .HasMaxLength(100)
            .IsUnicode(false);


        // ============================================================
        // Manufacturer
        // ============================================================

        builder.Property(a => a.Manufacturer)
            .HasMaxLength(150)
            .IsUnicode(true);


        // ============================================================
        // Model
        // ============================================================

        builder.Property(a => a.Model)
            .HasMaxLength(150)
            .IsUnicode(true);


        // ============================================================
        // Foreign Keys
        // ============================================================

        builder.Property(a => a.AssetCategoryId)
            .IsRequired();

        builder.Property(a => a.PurchaseId)
            .IsRequired();

        builder.Property(a => a.DepartmentId)
            .IsRequired(false);


        // ============================================================
        // Purchase Information
        // ============================================================

        builder.Property(a => a.PurchaseCost)
            .HasPrecision(18, 2);

        builder.Property(a => a.PurchaseDate)
            .IsRequired();


        // ============================================================
        // Asset Status
        // ============================================================

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<int>();


        // ============================================================
        // Asset Condition
        // ============================================================

        builder.Property(a => a.Condition)
            .IsRequired()
            .HasConversion<int>();


        // ============================================================
        // Current Location
        // ============================================================

        builder.Property(a => a.Location)
            .HasMaxLength(250)
            .IsUnicode(true);


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(a => a.AssetTag)
            .IsUnique();

        builder.HasIndex(a => a.SerialNumber)
            .IsUnique();

        builder.HasIndex(a => a.AssetCategoryId);

        builder.HasIndex(a => a.PurchaseId);

        builder.HasIndex(a => a.DepartmentId);

        builder.HasIndex(a => a.Status);

        builder.HasIndex(a => a.Condition);


        // ============================================================
        // Asset → Asset Category
        // ============================================================

        builder.HasOne(a => a.AssetCategory)
            .WithMany(ac => ac.Assets)
            .HasForeignKey(a => a.AssetCategoryId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → Purchase
        // ============================================================

        builder.HasOne(a => a.Purchase)
            .WithMany(p => p.Assets)
            .HasForeignKey(a => a.PurchaseId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → Department
        // ============================================================

        builder.HasOne(a => a.Department)
            .WithMany(d => d.Assets)
            .HasForeignKey(a => a.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → QR Code
        // ============================================================

        builder.HasOne(a => a.QRCode)
            .WithOne(q => q.Asset)
            .HasForeignKey<QRCode>(q => q.AssetId)
            .OnDelete(DeleteBehavior.Cascade);


        // ============================================================
        // Asset → Barcode
        // ============================================================

        builder.HasOne(a => a.Barcode)
            .WithOne(b => b.Asset)
            .HasForeignKey<Barcode>(b => b.AssetId)
            .OnDelete(DeleteBehavior.Cascade);


        // ============================================================
        // Asset → Asset Requests
        // ============================================================

        builder.HasMany(a => a.AssetRequests)
            .WithOne(ar => ar.Asset)
            .HasForeignKey(ar => ar.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → Asset Assignments
        // ============================================================

        builder.HasMany(a => a.AssetAssignments)
            .WithOne(aa => aa.Asset)
            .HasForeignKey(aa => aa.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → Asset Transfers
        // ============================================================

        builder.HasMany(a => a.AssetTransfers)
            .WithOne(at => at.Asset)
            .HasForeignKey(at => at.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → Damage Reports
        // ============================================================

        builder.HasMany(a => a.DamageReports)
            .WithOne(dr => dr.Asset)
            .HasForeignKey(dr => dr.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → Maintenance
        // ============================================================

        builder.HasMany(a => a.Maintenance)
            .WithOne(m => m.Asset)
            .HasForeignKey(m => m.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → Asset Returns
        // ============================================================

        builder.HasMany(a => a.AssetReturns)
            .WithOne(ar => ar.Asset)
            .HasForeignKey(ar => ar.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset → Asset Disposal
        // ============================================================

        builder.HasMany(a => a.AssetDisposals)
            .WithOne(ad => ad.Asset)
            .HasForeignKey(ad => ad.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(a => a.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}