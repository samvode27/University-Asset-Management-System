using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.AssetRequests;

namespace UAMS.Infrastructure.Configurations;

public class AssetRequestConfiguration : IEntityTypeConfiguration<AssetRequest>
{
    public void Configure(EntityTypeBuilder<AssetRequest> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("AssetRequests");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(ar => ar.Id);

        builder.Property(ar => ar.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Request Number
        // ============================================================

        builder.Property(ar => ar.RequestNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Asset Foreign Key
        // ============================================================

        builder.Property(ar => ar.AssetId)
            .IsRequired();


        // ============================================================
        // Requester Foreign Key
        // ============================================================

        builder.Property(ar => ar.RequesterId)
            .IsRequired();


        // ============================================================
        // Department Foreign Key
        // ============================================================

        builder.Property(ar => ar.DepartmentId)
            .IsRequired();


        // ============================================================
        // Purpose
        // ============================================================

        builder.Property(ar => ar.Purpose)
            .IsRequired()
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Requested Date
        // ============================================================

        builder.Property(ar => ar.RequestedDate)
            .IsRequired();


        // ============================================================
        // Required From Date
        // ============================================================

        builder.Property(ar => ar.RequiredFromDate)
            .IsRequired(false);


        // ============================================================
        // Required To Date
        // ============================================================

        builder.Property(ar => ar.RequiredToDate)
            .IsRequired(false);


        // ============================================================
        // Request Status
        // ============================================================

        builder.Property(ar => ar.Status)
            .IsRequired();


        // ============================================================
        // Department Head
        // ============================================================

        builder.Property(ar => ar.DepartmentHeadId)
            .IsRequired(false);


        builder.Property(ar => ar.DepartmentHeadActionDate)
            .IsRequired(false);


        builder.Property(ar => ar.DepartmentHeadRemarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Asset Manager
        // ============================================================

        builder.Property(ar => ar.AssetManagerId)
            .IsRequired(false);


        builder.Property(ar => ar.AssetManagerActionDate)
            .IsRequired(false);


        builder.Property(ar => ar.AssetManagerRemarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Rejection Reason
        // ============================================================

        builder.Property(ar => ar.RejectionReason)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(ar => ar.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(ar => ar.RequestNumber)
            .IsUnique();

        builder.HasIndex(ar => ar.AssetId);

        builder.HasIndex(ar => ar.RequesterId);

        builder.HasIndex(ar => ar.DepartmentId);

        builder.HasIndex(ar => ar.Status);

        builder.HasIndex(ar => ar.RequestedDate);


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasOne(ar => ar.Asset)
            .WithMany(a => a.AssetRequests)
            .HasForeignKey(ar => ar.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Requester Relationship
        // ============================================================

        builder.HasOne(ar => ar.Requester)
            .WithMany()
            .HasForeignKey(ar => ar.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Department Relationship
        // ============================================================

        builder.HasOne(ar => ar.Department)
            .WithMany(d => d.AssetRequests)
            .HasForeignKey(ar => ar.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Department Head Relationship
        // ============================================================

        builder.HasOne(ar => ar.DepartmentHead)
            .WithMany()
            .HasForeignKey(ar => ar.DepartmentHeadId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset Manager Relationship
        // ============================================================

        builder.HasOne(ar => ar.AssetManager)
            .WithMany()
            .HasForeignKey(ar => ar.AssetManagerId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(ar => ar.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(ar => !ar.IsDeleted);
    }
}