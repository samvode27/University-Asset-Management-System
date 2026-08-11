using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.AssetTransfers;

namespace UAMS.Infrastructure.Configurations;

public class AssetTransferConfiguration
    : IEntityTypeConfiguration<AssetTransfer>
{
    public void Configure(EntityTypeBuilder<AssetTransfer> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("AssetTransfers");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(at => at.Id);

        builder.Property(at => at.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Transfer Number
        // ============================================================

        builder.Property(at => at.TransferNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Asset Foreign Key
        // ============================================================

        builder.Property(at => at.AssetId)
            .IsRequired();


        // ============================================================
        // Asset Assignment Foreign Key
        // ============================================================

        builder.Property(at => at.AssetAssignmentId)
            .IsRequired();


        // ============================================================
        // Requested By Foreign Key
        // ============================================================

        builder.Property(at => at.RequestedById)
            .IsRequired();


        // ============================================================
        // From Employee Foreign Key
        // ============================================================

        builder.Property(at => at.FromEmployeeId)
            .IsRequired();


        // ============================================================
        // To Employee Foreign Key
        // ============================================================

        builder.Property(at => at.ToEmployeeId)
            .IsRequired();


        // ============================================================
        // From Department Foreign Key
        // ============================================================

        builder.Property(at => at.FromDepartmentId)
            .IsRequired();


        // ============================================================
        // To Department Foreign Key
        // ============================================================

        builder.Property(at => at.ToDepartmentId)
            .IsRequired();


        // ============================================================
        // From Location
        // ============================================================

        builder.Property(at => at.FromLocation)
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // To Location
        // ============================================================

        builder.Property(at => at.ToLocation)
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Reason
        // ============================================================

        builder.Property(at => at.Reason)
            .IsRequired()
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Requested Date
        // ============================================================

        builder.Property(at => at.RequestedDate)
            .IsRequired();


        // ============================================================
        // Approval Information
        // ============================================================

        builder.Property(at => at.ApprovedById)
            .IsRequired(false);


        builder.Property(at => at.ApprovedDate)
            .IsRequired(false);


        builder.Property(at => at.ApprovalRemarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Completed Date
        // ============================================================

        builder.Property(at => at.CompletedDate)
            .IsRequired(false);


        // ============================================================
        // Remarks
        // ============================================================

        builder.Property(at => at.Remarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Transfer Status
        // ============================================================

        builder.Property(at => at.Status)
            .IsRequired();


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(at => at.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(at => at.TransferNumber)
            .IsUnique();

        builder.HasIndex(at => at.AssetId);

        builder.HasIndex(at => at.AssetAssignmentId);

        builder.HasIndex(at => at.RequestedById);

        builder.HasIndex(at => at.FromEmployeeId);

        builder.HasIndex(at => at.ToEmployeeId);

        builder.HasIndex(at => at.FromDepartmentId);

        builder.HasIndex(at => at.ToDepartmentId);

        builder.HasIndex(at => at.Status);

        builder.HasIndex(at => at.RequestedDate);


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasOne(at => at.Asset)
            .WithMany(a => a.AssetTransfers)
            .HasForeignKey(at => at.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset Assignment Relationship
        // ============================================================

        builder.HasOne(at => at.AssetAssignment)
            .WithMany()
            .HasForeignKey(at => at.AssetAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Requested By Relationship
        // ============================================================

        builder.HasOne(at => at.RequestedBy)
            .WithMany()
            .HasForeignKey(at => at.RequestedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // From Employee Relationship
        // ============================================================

        builder.HasOne(at => at.FromEmployee)
            .WithMany()
            .HasForeignKey(at => at.FromEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // To Employee Relationship
        // ============================================================

        builder.HasOne(at => at.ToEmployee)
            .WithMany()
            .HasForeignKey(at => at.ToEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // From Department Relationship
        // ============================================================

        builder.HasOne(at => at.FromDepartment)
            .WithMany()
            .HasForeignKey(at => at.FromDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // To Department Relationship
        // ============================================================

        builder.HasOne(at => at.ToDepartment)
            .WithMany()
            .HasForeignKey(at => at.ToDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Approved By Relationship
        // ============================================================

        builder.HasOne(at => at.ApprovedBy)
            .WithMany()
            .HasForeignKey(at => at.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(at => at.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(at => !at.IsDeleted);
    }
}