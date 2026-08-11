using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.Maintenances;

namespace UAMS.Infrastructure.Configurations;

public class MaintenanceConfiguration : IEntityTypeConfiguration<Maintenance>
{
    public void Configure(EntityTypeBuilder<Maintenance> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("Maintenances");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Maintenance Number
        // ============================================================

        builder.Property(m => m.MaintenanceNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Foreign Keys
        // ============================================================

        builder.Property(m => m.AssetId)
            .IsRequired();

        // DamageReportId is nullable in the entity
        builder.Property(m => m.DamageReportId)
            .IsRequired(false);

        builder.Property(m => m.RequestedById)
            .IsRequired();

        builder.Property(m => m.AssignedTechnicianId)
            .IsRequired(false);


        // ============================================================
        // Maintenance Type
        // ============================================================

        builder.Property(m => m.MaintenanceType)
            .IsRequired();


        // ============================================================
        // Problem Information
        // ============================================================

        builder.Property(m => m.ProblemDescription)
            .IsRequired()
            .HasMaxLength(2000)
            .IsUnicode(true);

        builder.Property(m => m.MaintenanceDescription)
            .HasMaxLength(2000)
            .IsUnicode(true);

        builder.Property(m => m.PartsUsed)
            .HasMaxLength(2000)
            .IsUnicode(true);

        builder.Property(m => m.FailureReason)
            .HasMaxLength(2000)
            .IsUnicode(true);

        builder.Property(m => m.Remarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Cost Information
        // ============================================================

        builder.Property(m => m.EstimatedCost)
            .HasPrecision(18, 2);

        builder.Property(m => m.ActualCost)
            .HasPrecision(18, 2);


        // ============================================================
        // Dates
        // ============================================================

        builder.Property(m => m.RequestedDate)
            .IsRequired();

        builder.Property(m => m.StartedDate)
            .IsRequired(false);

        builder.Property(m => m.CompletedDate)
            .IsRequired(false);


        // ============================================================
        // Maintenance Result
        // ============================================================

        builder.Property(m => m.Result)
            .IsRequired(false);


        // ============================================================
        // Maintenance Status
        // ============================================================

        builder.Property(m => m.Status)
            .IsRequired();


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(m => m.MaintenanceNumber)
            .IsUnique();

        builder.HasIndex(m => m.AssetId);

        builder.HasIndex(m => m.DamageReportId);

        builder.HasIndex(m => m.RequestedById);

        builder.HasIndex(m => m.AssignedTechnicianId);

        builder.HasIndex(m => m.Status);

        builder.HasIndex(m => m.RequestedDate);


        // ============================================================
        // Damage Report Relationship
        // ============================================================

        builder.HasOne(m => m.DamageReport)
            .WithMany()
            .HasForeignKey(m => m.DamageReportId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Requesting User Relationship
        // ============================================================

        builder.HasOne(m => m.RequestedBy)
            .WithMany()
            .HasForeignKey(m => m.RequestedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Assigned Technician Relationship
        // ============================================================

        builder.HasOne(m => m.AssignedTechnician)
            .WithMany()
            .HasForeignKey(m => m.AssignedTechnicianId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(m => m.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}