using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.DamageReports;

namespace UAMS.Infrastructure.Configurations;

public class DamageReportConfiguration
    : IEntityTypeConfiguration<DamageReport>
{
    public void Configure(EntityTypeBuilder<DamageReport> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("DamageReports");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(dr => dr.Id);

        builder.Property(dr => dr.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Report Number
        // ============================================================

        builder.Property(dr => dr.ReportNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);


        // ============================================================
        // Asset Foreign Key
        // ============================================================

        builder.Property(dr => dr.AssetId)
            .IsRequired();


        // ============================================================
        // Asset Assignment Foreign Key
        // ============================================================

        builder.Property(dr => dr.AssetAssignmentId)
            .IsRequired();


        // ============================================================
        // Reported By Foreign Key
        // ============================================================

        builder.Property(dr => dr.ReportedById)
            .IsRequired();


        // ============================================================
        // Reported Date
        // ============================================================

        builder.Property(dr => dr.ReportedDate)
            .IsRequired();


        // ============================================================
        // Damage Type
        // ============================================================

        builder.Property(dr => dr.DamageType)
            .IsRequired();


        // ============================================================
        // Damage Severity
        // ============================================================

        builder.Property(dr => dr.Severity)
            .IsRequired();


        // ============================================================
        // Description
        // ============================================================

        builder.Property(dr => dr.Description)
            .IsRequired()
            .HasMaxLength(2000)
            .IsUnicode(true);


        // ============================================================
        // Incident Date
        // ============================================================

        builder.Property(dr => dr.IncidentDate)
            .IsRequired(false);


        // ============================================================
        // Incident Location
        // ============================================================

        builder.Property(dr => dr.IncidentLocation)
            .HasMaxLength(500)
            .IsUnicode(true);


        // ============================================================
        // Repairable
        // ============================================================

        builder.Property(dr => dr.IsRepairable)
            .IsRequired(false);


        // ============================================================
        // Assessment
        // ============================================================

        builder.Property(dr => dr.Assessment)
            .HasMaxLength(2000)
            .IsUnicode(true);


        // ============================================================
        // Assessed By
        // ============================================================

        builder.Property(dr => dr.AssessedById)
            .IsRequired(false);


        builder.Property(dr => dr.AssessedDate)
            .IsRequired(false);


        // ============================================================
        // Damage Status
        // ============================================================

        builder.Property(dr => dr.Status)
            .IsRequired();


        // ============================================================
        // Resolution
        // ============================================================

        builder.Property(dr => dr.ResolutionRemarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        builder.Property(dr => dr.ResolvedDate)
            .IsRequired(false);


        // ============================================================
        // Remarks
        // ============================================================

        builder.Property(dr => dr.Remarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Active Status
        // ============================================================

        builder.Property(dr => dr.IsActive)
            .IsRequired()
            .HasDefaultValue(true);


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(dr => dr.ReportNumber)
            .IsUnique();

        builder.HasIndex(dr => dr.AssetId);

        builder.HasIndex(dr => dr.AssetAssignmentId);

        builder.HasIndex(dr => dr.ReportedById);

        builder.HasIndex(dr => dr.AssessedById);

        builder.HasIndex(dr => dr.Status);

        builder.HasIndex(dr => dr.ReportedDate);


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasOne(dr => dr.Asset)
            .WithMany(a => a.DamageReports)
            .HasForeignKey(dr => dr.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset Assignment Relationship
        // ============================================================

        builder.HasOne(dr => dr.AssetAssignment)
            .WithMany()
            .HasForeignKey(dr => dr.AssetAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Reported By Relationship
        // ============================================================

        builder.HasOne(dr => dr.ReportedBy)
            .WithMany()
            .HasForeignKey(dr => dr.ReportedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Assessed By Relationship
        // ============================================================

        builder.HasOne(dr => dr.AssessedBy)
            .WithMany()
            .HasForeignKey(dr => dr.AssessedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Soft Delete
        // ============================================================

        builder.Property(dr => dr.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(dr => !dr.IsDeleted);
    }
}