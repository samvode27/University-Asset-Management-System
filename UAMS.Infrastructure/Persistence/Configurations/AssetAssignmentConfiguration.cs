using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.AssetAssignments;

namespace UAMS.Infrastructure.Configurations;

public class AssetAssignmentConfiguration
    : IEntityTypeConfiguration<AssetAssignment>
{
    public void Configure(EntityTypeBuilder<AssetAssignment> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("AssetAssignments");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(aa => aa.Id);

        builder.Property(aa => aa.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Assignment Number
        // ============================================================

        builder.Property(aa => aa.AssignmentNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Asset Foreign Key
        // ============================================================

        builder.Property(aa => aa.AssetId)
            .IsRequired();


        // ============================================================
        // Asset Request Foreign Key
        // ============================================================

        builder.Property(aa => aa.AssetRequestId)
            .IsRequired();


        // ============================================================
        // Employee Foreign Key
        // ============================================================

        builder.Property(aa => aa.EmployeeId)
            .IsRequired();


        // ============================================================
        // Assigned By Foreign Key
        // ============================================================

        builder.Property(aa => aa.AssignedById)
            .IsRequired();


        // ============================================================
        // Assignment Date
        // ============================================================

        builder.Property(aa => aa.AssignedDate)
            .IsRequired();


        // ============================================================
        // Expected Return Date
        // ============================================================

        builder.Property(aa => aa.ExpectedReturnDate)
            .IsRequired(false);


        // ============================================================
        // Actual Return Date
        // ============================================================

        builder.Property(aa => aa.ActualReturnDate)
            .IsRequired(false);


        // ============================================================
        // Assignment Location
        // ============================================================

        builder.Property(aa => aa.AssignmentLocation)
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Condition At Assignment
        // ============================================================

        builder.Property(aa => aa.ConditionAtAssignment)
            .IsRequired();


        // ============================================================
        // Remarks
        // ============================================================

        builder.Property(aa => aa.Remarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Assignment Status
        // ============================================================

        builder.Property(aa => aa.Status)
            .IsRequired();


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(aa => aa.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(aa => aa.AssignmentNumber)
            .IsUnique();

        builder.HasIndex(aa => aa.AssetId);

        builder.HasIndex(aa => aa.AssetRequestId);

        builder.HasIndex(aa => aa.EmployeeId);

        builder.HasIndex(aa => aa.AssignedById);

        builder.HasIndex(aa => aa.Status);

        builder.HasIndex(aa => aa.AssignedDate);


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasOne(aa => aa.Asset)
            .WithMany(a => a.AssetAssignments)
            .HasForeignKey(aa => aa.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset Request Relationship
        // ============================================================

        builder.HasOne(aa => aa.AssetRequest)
            .WithMany()
            .HasForeignKey(aa => aa.AssetRequestId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Employee Relationship
        // ============================================================

        builder.HasOne(aa => aa.Employee)
            .WithMany()
            .HasForeignKey(aa => aa.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Assigned By Relationship
        // ============================================================

        builder.HasOne(aa => aa.AssignedBy)
            .WithMany()
            .HasForeignKey(aa => aa.AssignedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(aa => aa.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(aa => !aa.IsDeleted);
    }
}