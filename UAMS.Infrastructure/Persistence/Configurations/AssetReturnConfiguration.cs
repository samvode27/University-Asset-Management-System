using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UAMS.Domain.Entities.AssetReturns;

namespace UAMS.Infrastructure.Configurations;

public class AssetReturnConfiguration : IEntityTypeConfiguration<AssetReturn>
{
    public void Configure(EntityTypeBuilder<AssetReturn> builder)
    {
        // ============================================================
        // Table
        // ============================================================

        builder.ToTable("AssetReturns");


        // ============================================================
        // Primary Key
        // ============================================================

        builder.HasKey(ar => ar.Id);

        builder.Property(ar => ar.Id)
            .ValueGeneratedNever();


        // ============================================================
        // Return Number
        // ============================================================

        builder.Property(ar => ar.ReturnNumber)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(true);

        builder.HasIndex(ar => ar.ReturnNumber)
            .IsUnique();


        // ============================================================
        // Foreign Keys
        // ============================================================

        builder.Property(ar => ar.AssetId)
            .IsRequired();

        builder.Property(ar => ar.AssetAssignmentId)
            .IsRequired();

        builder.Property(ar => ar.ReturnedById)
            .IsRequired();

        builder.Property(ar => ar.ReceivedById)
            .IsRequired();

        builder.Property(ar => ar.InspectedById)
            .IsRequired(false);

        builder.Property(ar => ar.DamageReportId)
            .IsRequired(false);


        // ============================================================
        // Return Information
        // ============================================================

        builder.Property(ar => ar.ReturnDate)
            .IsRequired();

        builder.Property(ar => ar.ReturnLocation)
            .HasMaxLength(500)
            .IsUnicode(true);

        builder.Property(ar => ar.Condition)
            .IsRequired();

        builder.Property(ar => ar.InspectionNotes)
            .HasMaxLength(2000)
            .IsUnicode(true);

        builder.Property(ar => ar.DamageFound)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ar => ar.Remarks)
            .HasMaxLength(1000)
            .IsUnicode(true);


        // ============================================================
        // Inspection Information
        // ============================================================

        builder.Property(ar => ar.InspectionDate)
            .IsRequired(false);


        // ============================================================
        // Return Status
        // ============================================================

        builder.Property(ar => ar.Status)
            .IsRequired();


        // ============================================================
        // Indexes
        // ============================================================

        builder.HasIndex(ar => ar.AssetId);

        builder.HasIndex(ar => ar.AssetAssignmentId);

        builder.HasIndex(ar => ar.ReturnedById);

        builder.HasIndex(ar => ar.ReceivedById);

        builder.HasIndex(ar => ar.InspectedById);

        builder.HasIndex(ar => ar.DamageReportId);

        builder.HasIndex(ar => ar.Status);

        builder.HasIndex(ar => ar.ReturnDate);


        // ============================================================
        // Asset Relationship
        // ============================================================

        builder.HasOne(ar => ar.Asset)
            .WithMany(a => a.AssetReturns)
            .HasForeignKey(ar => ar.AssetId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Asset Assignment Relationship
        // ============================================================
        // AssetAssignment does not currently contain an AssetReturns
        // collection, so WithMany() is used.

        builder.HasOne(ar => ar.AssetAssignment)
            .WithMany()
            .HasForeignKey(ar => ar.AssetAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Returned By User Relationship
        // ============================================================
        // No inverse collection is required on User.

        builder.HasOne(ar => ar.ReturnedBy)
            .WithMany()
            .HasForeignKey(ar => ar.ReturnedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Received By User Relationship
        // ============================================================
        // No inverse collection is required on User.

        builder.HasOne(ar => ar.ReceivedBy)
            .WithMany()
            .HasForeignKey(ar => ar.ReceivedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Inspected By User Relationship
        // ============================================================

        builder.HasOne(ar => ar.InspectedBy)
            .WithMany()
            .HasForeignKey(ar => ar.InspectedById)
            .OnDelete(DeleteBehavior.Restrict);


        // ============================================================
        // Damage Report Relationship
        // ============================================================

        builder.HasOne(ar => ar.DamageReport)
            .WithMany()
            .HasForeignKey(ar => ar.DamageReportId)
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
