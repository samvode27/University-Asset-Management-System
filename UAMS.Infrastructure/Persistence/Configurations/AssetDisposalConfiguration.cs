using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.AssetDisposals;

namespace UAMS.Infrastructure.Configurations;

public class AssetDisposalConfiguration : IEntityTypeConfiguration<AssetDisposal>
{
    public void Configure(EntityTypeBuilder<AssetDisposal> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("AssetDisposals");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(ad => ad.Id);

        builder.Property(ad => ad.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Disposal Number
        // ============================================================

        builder.Property(ad => ad.DisposalNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Foreign Keys
        // ============================================================

        builder.Property(ad => ad.AssetId)
            .IsRequired();

        builder.Property(ad => ad.MaintenanceId);

        builder.Property(ad => ad.RequestedById)
            .IsRequired();

        builder.Property(ad => ad.ApprovedById);

        builder.Property(ad => ad.CompletedById);


        // ============================================================
        // Disposal Information
        // ============================================================

        builder.Property(ad => ad.Reason)
            .IsRequired()
            .HasMaxLength(1000)
            .IsUnicode(true);

        builder.Property(ad => ad.BookValue)
            .HasPrecision(18, 2);

        builder.Property(ad => ad.EstimatedValue)
            .HasPrecision(18, 2);

        builder.Property(ad => ad.DisposalValue)
            .HasPrecision(18, 2);

        builder.Property(ad => ad.DisposalMethod)
            .HasConversion<int?>();

        builder.Property(ad => ad.Remarks)
            .HasMaxLength(2000)
            .IsUnicode(true);


        // ============================================================
        // Dates
        // ============================================================

        builder.Property(ad => ad.RequestedDate)
            .IsRequired();

        builder.Property(ad => ad.ApprovedDate);

        builder.Property(ad => ad.DisposalDate);


        // ============================================================
        // Status
        // ============================================================

        builder.Property(ad => ad.Status)
            .IsRequired()
            .HasConversion<int>();


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(ad => ad.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(ad => ad.DisposalNumber)
            .IsUnique();

        builder.HasIndex(ad => ad.AssetId);

        builder.HasIndex(ad => ad.MaintenanceId);

        builder.HasIndex(ad => ad.RequestedById);

        builder.HasIndex(ad => ad.ApprovedById);

        builder.HasIndex(ad => ad.CompletedById);

        builder.HasIndex(ad => ad.Status);

        builder.HasIndex(ad => ad.RequestedDate);

        builder.HasIndex(ad => ad.ApprovedDate);

        builder.HasIndex(ad => ad.DisposalDate);


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasOne(ad => ad.Asset)
            .WithMany(a => a.AssetDisposals)
            .HasForeignKey(ad => ad.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Maintenance Relationship
        // ============================================================

        builder.HasOne(ad => ad.Maintenance)
            .WithMany()
            .HasForeignKey(ad => ad.MaintenanceId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Requesting User Relationship
        // ============================================================

        builder.HasOne(ad => ad.RequestedBy)
            .WithMany()
            .HasForeignKey(ad => ad.RequestedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Approving User Relationship
        // ============================================================

        builder.HasOne(ad => ad.ApprovedBy)
            .WithMany()
            .HasForeignKey(ad => ad.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Completing User Relationship
        // ============================================================

        builder.HasOne(ad => ad.CompletedBy)
            .WithMany()
            .HasForeignKey(ad => ad.CompletedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(ad => ad.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(ad => !ad.IsDeleted);
    }
}